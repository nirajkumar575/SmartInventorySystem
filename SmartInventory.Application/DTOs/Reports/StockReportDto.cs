namespace SmartInventory.Application.DTOs.Reports;

public class StockReportDto
{
    public string ProductName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal StockValue { get; set; }
}