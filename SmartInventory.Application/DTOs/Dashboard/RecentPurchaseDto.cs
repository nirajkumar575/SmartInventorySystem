namespace SmartInventory.Application.DTOs.Dashboard;

public class RecentPurchaseDto
{
    public string InvoiceNumber { get; set; } = "";
    public string SupplierName { get; set; } = "";
    public decimal TotalAmount { get; set; }
    public DateTime PurchaseDate { get; set; }
}