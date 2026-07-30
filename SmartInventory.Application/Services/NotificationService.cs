using AutoMapper;
using SmartInventory.Application.DTOs.Notification;
using SmartInventory.Application.Interfaces;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public NotificationService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<List<NotificationDto>> GetAllAsync()
    {
        var notifications = await _unitOfWork.Notifications
            .GetAllAsync(_currentUser.UserId);

        return _mapper.Map<List<NotificationDto>>(notifications);
    }

    public async Task<int> GetUnreadCountAsync()
    {
        return await _unitOfWork.Notifications
            .GetUnreadCountAsync(_currentUser.UserId);
    }

    public async Task CreateAsync(CreateNotificationDto dto)
    {
        var notification = new Notification
        {
            Title = dto.Title,
            Message = dto.Message,
            Type = dto.Type,
            Url = dto.Url,
            UserId = dto.UserId,
            CreatedOn = DateTime.UtcNow,
            IsRead = false
        };

        await _unitOfWork.Notifications.AddAsync(notification);

        await _unitOfWork.Notifications.SaveChangesAsync();
    }

    public async Task<bool> MarkAsReadAsync(int id)
    {
        var notification = await _unitOfWork.Notifications.GetByIdAsync(id);

        if (notification == null)
            return false;

        notification.IsRead = true;

        await _unitOfWork.Notifications.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var notification = await _unitOfWork.Notifications.GetByIdAsync(id);

        if (notification == null)
            return false;

        _unitOfWork.Notifications.Remove(notification);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
    public async Task<NotificationDashboardDto> GetDashboardAsync()
    {
        var notifications = await _unitOfWork.Notifications
            .GetAllAsync(_currentUser.UserId);

        var unreadCount = await _unitOfWork.Notifications
            .GetUnreadCountAsync(_currentUser.UserId);

        return new NotificationDashboardDto
        {
            UnreadCount = unreadCount,
            Notifications = _mapper.Map<List<NotificationDto>>(notifications)
        };
    }
}