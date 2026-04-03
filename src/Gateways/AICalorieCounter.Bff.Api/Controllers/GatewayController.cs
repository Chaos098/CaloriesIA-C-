using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace AICalorieCounter.Bff.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GatewayController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public GatewayController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [HttpGet("services")]
    public IActionResult GetServiceUrls() => Ok(_configuration.GetSection("ServiceUrls").GetChildren().ToDictionary(x => x.Key, x => x.Value));

    [HttpGet("nutrition/{name}")]
    public async Task<IActionResult> GetNutritionByName(string name, CancellationToken cancellationToken)
        => await Forward(HttpMethod.Get, $"NutritionService/api/nutrition/{Uri.EscapeDataString(name)}", null, cancellationToken);

    [HttpPost("image/analyze")]
    public async Task<IActionResult> AnalyzeImage([FromBody] JsonElement payload, CancellationToken cancellationToken)
        => await Forward(HttpMethod.Post, "ImageAnalysisService/api/imageanalysis/analyze", payload, cancellationToken);

    [HttpPost("calories/compute")]
    public async Task<IActionResult> ComputeCalories([FromBody] JsonElement payload, CancellationToken cancellationToken)
        => await Forward(HttpMethod.Post, "CalorieComputationService/api/caloriecomputation/compute", payload, cancellationToken);

    [HttpPost("notifications/send")]
    public async Task<IActionResult> SendNotification([FromBody] JsonElement payload, CancellationToken cancellationToken)
        => await Forward(HttpMethod.Post, "PushNotificationService/api/pushnotifications/send", payload, cancellationToken);

    private async Task<IActionResult> Forward(HttpMethod method, string route, JsonElement? payload, CancellationToken cancellationToken)
    {
        var parts = route.Split('/', 2);
        if (parts.Length != 2)
        {
            return BadRequest("Invalid route.");
        }

        var serviceName = parts[0];
        var relativePath = parts[1];
        var baseUrl = _configuration[$"ServiceUrls:{serviceName}"];

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Service URL is missing for '{serviceName}'.");
        }

        var client = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage(method, $"{baseUrl.TrimEnd('/')}/{relativePath}");

        if (payload.HasValue)
        {
            request.Content = new StringContent(payload.Value.GetRawText(), Encoding.UTF8, "application/json");
        }

        using var response = await client.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(body))
        {
            return StatusCode((int)response.StatusCode);
        }

        return Content(body, "application/json", Encoding.UTF8);
    }
}
