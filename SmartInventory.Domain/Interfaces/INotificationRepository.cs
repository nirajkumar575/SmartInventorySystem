using SmartInventory.Domain.Entities;

namespace SmartInventory.Domain.Interfaces;

public interface INotificationRepository
{
    Task<List<Notification>> GetAllAsync(string? userId);
    Task<int> GetUnreadCountAsync(string? userId);
    Task AddAsync(Notification notification);
    Task<Notification?> GetByIdAsync(int id);
    Task SaveChangesAsync();
    void Remove(Notification notification);
}