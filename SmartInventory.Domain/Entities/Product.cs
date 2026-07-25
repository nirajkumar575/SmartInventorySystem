using SmartInventory.Domain.Common;

namespace SmartInventory.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string SKU { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Quantity { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public ICollection<PurchaseItem>? PurchaseItems { get; set; }=new List<PurchaseItem>();
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}