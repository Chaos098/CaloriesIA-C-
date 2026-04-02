using ImageAnalysisService.Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ImageAnalysisService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageAnalysisController : ControllerBase
{
    [HttpPost("analyze")]
    public ActionResult<AnalyzeImageResponseDto> AnalyzeImage([FromBody] AnalyzeImageRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.ImageUrl))
        {
            return BadRequest("ImageUrl is required.");
        }

        // Stubbed response for local development until AI integration is added.
        var recognizedItems = new List<string> { "apple", "chicken breast", "rice" };
        return Ok(new AnalyzeImageResponseDto(request.ImageUrl, recognizedItems));
    }
}
