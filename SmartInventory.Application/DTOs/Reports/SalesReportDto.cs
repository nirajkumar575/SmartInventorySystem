namespace SmartInventory.Application.DTOs.Reports;

public class SalesReportDto
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}