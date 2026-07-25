namespace SmartInventory.Application.DTOs.Sale;

public class SaleDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<SaleItemDto> Items { get; set; } = new();
}