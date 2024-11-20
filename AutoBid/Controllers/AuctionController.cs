using AutoBid.Data;
using AutoBid.Dtos.Auction;
using AutoBid.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AutoBid.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        // Injecting dependencies via the constructor
        public AuctionController(DataContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Existing endpoint to get all auctions
        [HttpGet, Authorize]
        public IActionResult GetAuctions()
        {
            return Ok(_context.Auctions.Include(c => c.Seller).ToList());
        }

        // Existing endpoint to create an auction
        [HttpPost("create"), Authorize]
        public async Task<IActionResult> CreateAuction([FromBody] CreateAuctionDto dto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound("User not found");

            var auction = new Auction
            {
                Title = dto.Title,
                Description = dto.Description,
                AuctionImage = dto.AuctionImage,
                AuctionCategory = dto.AuctionCategory,
                SellerId = user.Id,  // Assuming SellerId is an integer
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                StartingBid = dto.StartingBid,
                WinningBid = dto.WinningBid,
                Status = dto.Status
            };

            _context.Auctions.Add(auction);
            await _context.SaveChangesAsync();

            return Ok(auction);
        }

        // New endpoint to get auctions by the logged-in user's seller ID
        [HttpGet("user-auctions"), Authorize]
        public async Task<IActionResult> GetUserAuctions()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized("User not authenticated");

            var auctions = await _context.Auctions
                .Where(a => a.SellerId == userId)
                .Include(c => c.Seller) // Include the seller information if needed
                .ToListAsync();

            if (auctions.Count == 0)
                return NotFound("No auctions found for this user");

            return Ok(auctions);
        }

        // GET: api/Auction/{id}
        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> GetAuctionById(int id)
        {
            var auction = await _context.Auctions
                .Include(a => a.Seller) // Include seller information if necessary
                .FirstOrDefaultAsync(a => a.AuctionId == id); // Change to your auction ID property

            if (auction == null)
            {
                return NotFound("Auction not found"); // Return 404 if not found
            }

            return Ok(auction); // Return the auction details
        }

        // New endpoint for updating an auction
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> UpdateAuction(int id, [FromBody] CreateAuctionDto auctionDto)
        {
            if (auctionDto == null || id <= 0)
            {
                return BadRequest("Invalid auction data.");
            }

            var existingAuction = await _context.Auctions.FindAsync(id);
            if (existingAuction == null)
            {
                return NotFound("Auction not found.");
            }

            // Update the properties
            existingAuction.Title = auctionDto.Title;
            existingAuction.Description = auctionDto.Description;
            existingAuction.StartTime = auctionDto.StartTime;
            existingAuction.EndTime = auctionDto.EndTime;
            existingAuction.StartingBid = auctionDto.StartingBid;

            _context.Entry(existingAuction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuctionExists(id))
                {
                    return NotFound("Auction not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Return a 204 No Content response
        }

        // New endpoint to get UserId by email
        [HttpGet("userid-by-email/{email}")]
        public async Task<IActionResult> GetUserIdByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return NotFound("User not found");

            return Ok(new { UserId = user.Id }); // Return the UserId
        }

        // New DELETE endpoint to delete an auction
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteAuction(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction == null)
            {
                return NotFound("Auction not found");
            }

            _context.Auctions.Remove(auction);
            await _context.SaveChangesAsync();

            return NoContent(); // Returns 204 No Content if successful
        }

        // Helper method to check if an auction exists
        private bool AuctionExists(int id)
        {
            return _context.Auctions.Any(e => e.AuctionId == id);
        }
    }
}
