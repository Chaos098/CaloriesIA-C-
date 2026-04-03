using Microsoft.AspNetCore.Mvc;
using PushNotificationService.Api.Dtos;

namespace PushNotificationService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PushNotificationsController : ControllerBase
{
    [HttpPost("send")]
    public ActionResult<SendNotificationResponseDto> Send([FromBody] SendNotificationRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.DeviceToken))
        {
            return BadRequest("DeviceToken is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest("Title and Message are required.");
        }

        // Stubbed success response for now.
        return Ok(new SendNotificationResponseDto(true, "stub-provider", DateTimeOffset.UtcNow));
    }
}
