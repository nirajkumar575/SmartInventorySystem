using SmartInventory.Domain.Common;

namespace SmartInventory.Domain.Entities;

public class Sale : BaseEntity
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; } = DateTime.UtcNow;
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Completed";
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}