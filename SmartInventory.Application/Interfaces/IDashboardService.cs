using SmartInventory.Application.DTOs.Dashboard;

namespace SmartInventory.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync();
}