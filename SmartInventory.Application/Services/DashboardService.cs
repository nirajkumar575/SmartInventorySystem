using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs.Dashboard;
using SmartInventory.Application.Interfaces;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DashboardService> _logger;

    public DashboardService(
        IUnitOfWork unitOfWork,
        ILogger<DashboardService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<DashboardDto> GetDashboardAsync()
    {
        var today = DateTime.UtcNow.Date;

        var dashboard = new DashboardDto
        {
            TotalProducts =
                await _unitOfWork.ProductRepository.CountAsync(),

            TotalCategories =
                await _unitOfWork.CategoryRepository.CountAsync(),

            TotalSuppliers =
                await _unitOfWork.SupplierRepository.CountAsync(),

            TotalCustomers =
                await _unitOfWork.CustomerRepository.CountAsync(),

            TotalPurchases =
                await _unitOfWork.PurchaseRepository.CountAsync(),

            TotalSales =
                await _unitOfWork.SaleRepository.CountAsync(),

            LowStockProducts =
                await _unitOfWork.ProductRepository
                    .CountAsync(x => x.Quantity <= 10),

            TotalRevenue =
                await _unitOfWork.SaleRepository
                    .SumAsync(x => x.TotalAmount),

            TotalPurchaseAmount =
                await _unitOfWork.PurchaseRepository
                    .SumAsync(x => x.TotalAmount),

            TodaySales =
                await _unitOfWork.SaleRepository
                    .SumAsync(
                        x => x.SaleDate.Date == today,
                        x => x.TotalAmount),

            TodayPurchases =
                await _unitOfWork.PurchaseRepository
                    .SumAsync(
                        x => x.PurchaseDate.Date == today,
                        x => x.TotalAmount)
        };

        _logger.LogInformation("Dashboard loaded successfully.");

        return dashboard;
    }
}