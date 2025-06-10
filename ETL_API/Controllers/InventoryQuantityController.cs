using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ETL_API.Data;
using ETL_API.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ETL_API.Controllers
{
    [Route("api/inventory")]
    [ApiController]
    public class InventoryQuantityController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InventoryQuantityController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("currentInventory")]
        public async Task<IActionResult> GetInventory([FromBody] InventoryQueryRequest request, [FromHeader] string accessToken)
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

                var data = await _context.InventoryQuantities.ToListAsync();

                int page = request.Page > 0 ? request.Page : 1;
                int limit = request.Limit > 0 ? request.Limit : int.MaxValue;
                int skip = (page - 1) * limit;

                var grouped = data
                            .GroupBy(x => x.StockCode)
                            .ToList();

                var pagedGroups = grouped
                    .Skip(skip)
                    .Take(limit)
                    .ToList();

                var result = pagedGroups.Select(g => new
                {
                    stockCode = g.Key,
                    stockName = g.First().StockName,
                    locations = g.Select(x => new
                    {
                        locationCode = x.LocationCode,
                        quantity = x.InventoryQuantity
                    }).ToList()
                }).ToList();

                return Ok(new
                {
                    grouped.Count,
                    request.Page,
                    request.Limit,
                    message = result.Any() ? "Inventories fetched successfully." : "No inventories found.",
                    result
                });
            }
            catch (TaskCanceledException ex)
            {
                // Usually caused by timeouts
                Console.WriteLine("Timeout occurred: " + ex.Message);
                return StatusCode(504, "The request timed out. Please try again later.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
                return StatusCode(500, "Unexpected server error occurred: " + ex.Message); // Ok for dev
            }
        }

        public class InventoryQueryRequest
        {
            public int Page { get; set; }
            public int Limit { get; set; }
        }
    }
}
