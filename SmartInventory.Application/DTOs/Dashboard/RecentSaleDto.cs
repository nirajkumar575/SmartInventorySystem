namespace SmartInventory.Application.DTOs.Dashboard;

public class RecentSaleDto
{
    public string InvoiceNumber { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public decimal TotalAmount { get; set; }
    public DateTime SaleDate { get; set; }
}