using Microsoft.EntityFrameworkCore;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;
using SmartInventory.Infrastructure.Data;

namespace SmartInventory.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _context;

    public NotificationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Notification>> GetAllAsync(string? userId)
    {
        var query = _context.Notifications.AsQueryable();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            query = query.Where(x => x.UserId == userId);
        }

        return await query
            .OrderByDescending(x => x.CreatedOn)
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(string? userId)
    {
        var query = _context.Notifications.Where(x => !x.IsRead);

        if (!string.IsNullOrWhiteSpace(userId))
        {
            query = query.Where(x => x.UserId == userId);
        }

        return await query.CountAsync();
    }

    public async Task<Notification?> GetByIdAsync(int id)
    {
        return await _context.Notifications.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Notification notification)
    {
        await _context.Notifications.AddAsync(notification);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public void Remove(Notification notification)
    {
        _context.Notifications.Remove(notification);
    }
}