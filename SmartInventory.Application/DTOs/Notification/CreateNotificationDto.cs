namespace SmartInventory.Application.DTOs.Notification;

public class CreateNotificationDto
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = "Info";
    public string? UserId { get; set; }
    public string? Url { get; set; }
}