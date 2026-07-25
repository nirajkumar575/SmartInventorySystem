using SmartInventory.Domain.Common;

namespace SmartInventory.Domain.Entities;

public class Purchase : BaseEntity
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    public int SupplierId { get; set; }
    public Supplier Supplier { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Completed";
    public ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();
}