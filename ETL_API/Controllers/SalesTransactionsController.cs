using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ETL_API.Data;

namespace ETL_API.Controllers
{
    [Route("api/sales")]
    [ApiController]
    public class SalesTransactionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SalesTransactionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("transactions")]
        public async Task<IActionResult> GetTransactions([FromBody] SalesQueryRequest request, [FromHeader] string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                return Unauthorized("Access token is required.");

            var existingToken = await _context.Token
        .FirstOrDefaultAsync(t => t.TokenString == accessToken && t.IsActive);

            if (existingToken == null)
                return Unauthorized("Invalid or expired access token.");

            if (request.Limit > 50)
                return BadRequest("Limit cannot exceed 50.");

            if (request.Page < 1 || request.Limit < 1)
                return BadRequest("Page and limit must be greater than 0.");


            var filteredData = await _context.SalesTransactions
            .Where(t => t.IssueDate >= request.StartDate && t.IssueDate <= request.EndDate)
            .ToListAsync();

            var totalCount = filteredData.Count;

            var pagedData = filteredData
            .OrderBy(t => t.IssueDate)
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit)
                .ToList();

            return Ok(new
            {
                totalCount,
                request.Page,
                request.Limit,
                results = pagedData
            });
        }
    }

    public class SalesQueryRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
    }
}
