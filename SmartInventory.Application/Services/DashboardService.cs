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
            TotalProducts = await _unitOfWork.ProductRepository.CountAsync(),

            TotalCategories = await _unitOfWork.CategoryRepository.CountAsync(),

            TotalSuppliers = await _unitOfWork.SupplierRepository.CountAsync(),

            TotalCustomers = await _unitOfWork.CustomerRepository.CountAsync(),

            TotalSales = await _unitOfWork.SaleRepository.CountAsync(),

            TotalPurchases = await _unitOfWork.PurchaseRepository.CountAsync(),

            LowStockProducts = await _unitOfWork.ProductRepository.CountAsync(x => x.Quantity <= 10),

            TotalRevenue = await _unitOfWork.SaleRepository.SumAsync(x => x.TotalAmount),

            TotalPurchaseAmount = await _unitOfWork.PurchaseRepository.SumAsync(x => x.TotalAmount),

            TodaySales = await _unitOfWork.SaleRepository.SumAsync(
                x => x.SaleDate.Date == today,
                x => x.TotalAmount),

            TodayPurchases = await _unitOfWork.PurchaseRepository.SumAsync(
                x => x.PurchaseDate.Date == today,
                x => x.TotalAmount)
        };

        // Recent Sales
        var recentSales = await _unitOfWork.SaleRepository.GetRecentSalesAsync(5);

        dashboard.RecentSales = recentSales.Select(x => new RecentSaleDto
        {
            InvoiceNumber = x.InvoiceNumber,
            CustomerName = x.Customer.Name,
            TotalAmount = x.TotalAmount,
            SaleDate = x.SaleDate
        }).ToList();

        // Recent Purchases
        var recentPurchases = await _unitOfWork.PurchaseRepository.GetRecentPurchasesAsync(5);

        dashboard.RecentPurchases = recentPurchases.Select(x => new RecentPurchaseDto
        {
            InvoiceNumber = x.InvoiceNumber,
            SupplierName = x.Supplier.Name,
            TotalAmount = x.TotalAmount,
            PurchaseDate = x.PurchaseDate
        }).ToList();

        // Top Selling Products
        var topProducts = await _unitOfWork.SaleRepository.GetTopSellingProductsAsync(5);

        dashboard.TopSellingProducts = topProducts.Select(x => new TopSellingProductDto
        {
            ProductId = x.ProductId,
            ProductName = x.ProductName,
            QuantitySold = x.QuantitySold
        }).ToList();

        // Low Stock
        var lowStock = await _unitOfWork.ProductRepository.GetLowStockProductsAsync(10);

        dashboard.LowStockItems = lowStock.Select(x => new LowStockProductDto
        {
            ProductId = x.Id,
            ProductName = x.Name,
            Quantity = x.Quantity
        }).ToList();

        // Sales Chart
        var salesChart = await _unitOfWork.SaleRepository.GetLast7DaysSalesChartAsync();

        dashboard.SalesChart = salesChart.Select(x => new DashboardChartDto
        {
            Label = x.Date.ToString("dd MMM"),
            Value = x.TotalAmount
        }).ToList();

        // Purchase Chart
        var purchaseChart = await _unitOfWork.PurchaseRepository.GetLast7DaysPurchaseChartAsync();

        dashboard.PurchaseChart = purchaseChart.Select(x => new DashboardChartDto
        {
            Label = x.Date.ToString("dd MMM"),
            Value = x.TotalAmount
        }).ToList();

        _logger.LogInformation("Dashboard loaded successfully.");

        return dashboard;
    }
}