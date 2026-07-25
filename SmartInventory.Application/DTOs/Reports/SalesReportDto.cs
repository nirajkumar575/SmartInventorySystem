namespace SmartInventory.Application.DTOs.Reports;

public class SalesReportDto
{
    public int SaleId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<SalesReportItemDto> Items { get; set; } = new();
}