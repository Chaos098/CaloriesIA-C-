using CalorieComputationService.Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CalorieComputationService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalorieComputationController : ControllerBase
{
    [HttpPost("compute")]
    public ActionResult<ComputeCaloriesResponseDto> Compute([FromBody] ComputeCaloriesRequestDto request)
    {
        if (request.Items is null || request.Items.Count == 0)
        {
            return BadRequest("At least one nutrition item is required.");
        }

        var totalCalories = request.Items.Sum(i => i.Calories);
        var totalProtein = request.Items.Sum(i => i.Protein);
        var totalCarbohydrates = request.Items.Sum(i => i.Carbohydrates);
        var totalFat = request.Items.Sum(i => i.Fat);

        return Ok(new ComputeCaloriesResponseDto(totalCalories, totalProtein, totalCarbohydrates, totalFat));
    }
}
