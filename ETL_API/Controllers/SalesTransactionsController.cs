using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ETL_API.Data;
using ETL_API.Model;

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
        //public async Task<IActionResult> GetTransactions([FromBody] SalesQueryRequest request)
        //{
        public async Task<IActionResult> GetTransactions([FromBody] SalesQueryRequest request, [FromHeader] string accessToken)
        {
            try
            {
                if (string.IsNullOrEmpty(accessToken))
                    return Unauthorized("Access token is required.");

                var existingToken = await _context.Token
            .FirstOrDefaultAsync(t => t.TokenString == accessToken && t.IsActive);

                if (existingToken == null)
                    return Unauthorized("Invalid or expired access token.");

                if (request.Limit > 200)
                    return BadRequest("Limit cannot exceed 200.");

                if (request.Page < 1 || request.Limit < 1)
                    return BadRequest("Page and limit must be greater than 0.");

                var startDate = request.StartDate.Date;
                var endDate = request.EndDate.Date;

                var filteredData = await _context.SalesTransactions
                .Where(t => t.IssueDate >= startDate && t.IssueDate <= endDate)
                .ToListAsync();

                if (!filteredData.Any())
                {
                    return Ok(new
                    {
                        totalCount = 0,
                        request.Page,
                        request.Limit,
                        message = "No transactions found in the selected date range.",
                        results = new List<object>()
                    });
                }

                var totalCount = filteredData.Count;

                bool shouldIndicateShortenedDateRange = totalCount > (request.Page * request.Limit);

                string message = "All available results have been loaded.";

                if (shouldIndicateShortenedDateRange)
                    message = "The data is too large. Only partial results are shown. Please increase the page number to see more.";

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
                    message,
                    results = pagedData
                });
            }
            catch (TaskCanceledException ex)
            {
                // Usually caused by timeouts
                Console.WriteLine("Timeout occurred: " + ex.Message);
                return StatusCode(504, "The request timed out. Please try again or narrow your date range.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
                return StatusCode(500, "Unexpected server error occurred: " + ex.Message); // Ok for dev
            }
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
