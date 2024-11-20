
using AutoBid.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("getUserId")]
    [Authorize] // Ensure this is authorized
    public async Task<IActionResult> GetUserId()
    {
        var userEmail = User.Identity.Name; // This gets the email from the authenticated user
        var userId = await _userService.GetUserIdAsync(userEmail);

        if (userId == null)
        {
            return NotFound();
        }

        return Ok(userId);
    }

    

}

