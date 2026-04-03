namespace CalorieComputationService.Api.Dtos;

public record FoodNutritionItemDto(string Name, double Calories, double Protein, double Carbohydrates, double Fat);

public record ComputeCaloriesRequestDto(IReadOnlyList<FoodNutritionItemDto> Items);

public record ComputeCaloriesResponseDto(
    double TotalCalories,
    double TotalProtein,
    double TotalCarbohydrates,
    double TotalFat
);
