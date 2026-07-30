using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.DTOs.Notification;
using SmartInventory.Application.Interfaces;

namespace SmartInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var result = await _notificationService.GetDashboardAsync();

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _notificationService.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount()
    {
        var count = await _notificationService.GetUnreadCountAsync();

        return Ok(count);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateNotificationDto dto)
    {
        await _notificationService.CreateAsync(dto);

        return Ok(new
        {
            message = "Notification created successfully."
        });
    }

    [HttpPut("read/{id}")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var result = await _notificationService.MarkAsReadAsync(id);

        if (!result)
            return NotFound(new
            {
                message = "Notification not found."
            });

        return Ok(new
        {
            message = "Notification marked as read."
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _notificationService.DeleteAsync(id);

        if (!result)
            return NotFound(new
            {
                message = "Notification not found."
            });

        return Ok(new
        {
            message = "Notification deleted successfully."
        });
    }
}