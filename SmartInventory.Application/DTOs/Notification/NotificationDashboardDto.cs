namespace SmartInventory.Application.DTOs.Notification;

public class NotificationDashboardDto
{
    public int UnreadCount { get; set; }
    public List<NotificationDto> Notifications { get; set; } = [];
}