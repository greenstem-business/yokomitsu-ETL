using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ETL_API.Data;
using ETL_API.Model;
using System.Security.Cryptography;
using System.Text;

namespace ETL_API.Controllers
{
    [Route("api")]
    [ApiController]
    public class TokensController : Controller
    {
        private readonly AppDbContext _context;

        public TokensController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("getAccessToken")]
        public async Task<IActionResult> GetAccessToken(string companyName)
        {
            if (string.IsNullOrEmpty(companyName))
            {
                return BadRequest("Company name is required.");
            }

            var existingName = await _context.Company.FirstOrDefaultAsync(c => c.CompanyName == companyName);

            if (existingName == null)
            {
                return BadRequest("Company name does not exist in Greenplus.");
            }

            string partnerId = "p5JYmr154e3b+2VeWp5ngKASaLK71cJZaxjiHNWe6E/OjwFFRvpIgip/ZKf8rgpO";

            var existingToken = await _context.Token
                .FirstOrDefaultAsync(t => t.UserName == companyName);

            if (existingToken != null)
            {
                var tokenString = GenerateToken(companyName, partnerId);

                // Update the existing token with the new token string
                existingToken.TokenString = tokenString;
                existingToken.CreatedDateTime = DateTime.Now;

                _context.Update(existingToken);
                await _context.SaveChangesAsync();

                return Ok(tokenString);
            }

            var newTokenString = GenerateToken(companyName, partnerId);

            var newToken = new Token
            {
                UserName = companyName,
                TokenString = newTokenString,
                CreatedDateTime = DateTime.Now,
                IsActive = true
            };

            _context.Add(newToken);
            await _context.SaveChangesAsync();

            return Ok(newTokenString);
        }

        private string GenerateToken(string companyName, string partnerId)
        {
            using (var sha256 = SHA256.Create())
            {
                var rawData = companyName + partnerId + DateTime.UtcNow.ToString();
                var bytes = Encoding.UTF8.GetBytes(rawData);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
