using AutoBid.Data;
using AutoBid.Dtos.Bids;
using AutoBid.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace AutoBid.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BidController(DataContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        
        public IActionResult Bids => Ok(_context.Bids.ToList());
        [HttpPost("create"), Authorize]

        public async Task<IActionResult> CreateBid([FromBody] CreateBidDto dto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound("User not found");

            var bid = new Biding
            {
                vehiucleId = dto.vehiucleId,
                BidAmount = dto.BidAmount,
                userId = user.Id,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();

            return Ok(bid);
        }
        [HttpGet, Authorize]
        public IActionResult GetBids()
        {
            try
            {
                var bids = _context.Bids.ToList();
                return Ok(bids);
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging library)
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error while fetching bids.");
            }
        }

    }
}
