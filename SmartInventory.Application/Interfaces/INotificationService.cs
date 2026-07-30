using SmartInventory.Application.DTOs.Notification;

namespace SmartInventory.Application.Interfaces;

public interface INotificationService
{
    Task<List<NotificationDto>> GetAllAsync();
    Task<int> GetUnreadCountAsync();
    Task<bool> MarkAsReadAsync(int id);
    Task<bool> DeleteAsync(int id);
    Task CreateAsync(CreateNotificationDto dto);
    Task<NotificationDashboardDto> GetDashboardAsync();
}