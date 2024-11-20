using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class emailController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SendEmail()
        {
            return Ok(new { message = "Email sent successfully" });
        }

    }
}
