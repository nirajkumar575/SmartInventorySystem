namespace SmartInventory.Application.DTOs.Reports;

public class StockReportDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public bool IsLowStock { get; set; }
}