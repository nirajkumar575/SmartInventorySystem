namespace SmartInventory.Application.DTOs.Reports;

public class PurchaseReportDto
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}