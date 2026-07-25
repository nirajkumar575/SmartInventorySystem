namespace SmartInventory.Application.DTOs.Reports;

public class ProfitReportDto
{
    public decimal TotalPurchase { get; set; }
    public decimal TotalSales { get; set; }
    public decimal Profit { get; set; }
}