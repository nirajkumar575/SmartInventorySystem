namespace SmartInventory.Application.DTOs.Dashboard;

public class DashboardDto
{
    public int TotalProducts { get; set; }
    public int TotalCategories { get; set; }
    public int TotalSuppliers { get; set; }
    public int TotalCustomers { get; set; }
    public int TotalPurchases { get; set; }
    public int TotalSales { get; set; }
    public int LowStockProducts { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalPurchaseAmount { get; set; }
    public decimal TodaySales { get; set; }
    public decimal TodayPurchases { get; set; }
    public List<DashboardChartDto> SalesChart { get; set; } = new();
    public List<DashboardChartDto> PurchaseChart { get; set; } = new();
    public List<TopSellingProductDto> TopSellingProducts { get; set; } = new();
    public List<RecentSaleDto> RecentSales { get; set; } = new();
    public List<RecentPurchaseDto> RecentPurchases { get; set; } = new();
    public List<LowStockProductDto> LowStockItems { get; set; } = new();
}