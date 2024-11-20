using Authentication.PublicClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Background_Service
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            var fileName = new UploadHandler().Upload(file);

            if (fileName.Contains("not valid"))
            {
                return BadRequest(fileName); // Return error if file is not valid
            }

            // Construct the full URL to the uploaded file
            var fileUrl = $"{Request.Scheme}://{Request.Host}/api/Image/{fileName}";

            return Ok(fileUrl); // Return the URL of the uploaded image
        }

        [HttpGet("{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            var filePath = Path.Combine("Uploads", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Image not found");
            }

            // Determine MIME type based on file extension
            var fileExtension = Path.GetExtension(filePath).ToLower();
            var mimeType = fileExtension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream",  // Default for unknown types
            };

            var imageBytes = System.IO.File.ReadAllBytes(filePath);
            return File(imageBytes, mimeType);
        }

    }

}