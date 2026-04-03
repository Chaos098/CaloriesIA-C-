namespace PushNotificationService.Api.Dtos;

public record SendNotificationRequestDto(string DeviceToken, string Title, string Message);

public record SendNotificationResponseDto(bool Sent, string Provider, DateTimeOffset SentAtUtc);
