namespace ImageAnalysisService.Api.Dtos;

public record AnalyzeImageRequestDto(string ImageUrl);

public record AnalyzeImageResponseDto(string ImageUrl, IReadOnlyList<string> RecognizedFoodItems);
