using SmartInventory.Domain.Common;

namespace SmartInventory.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string SKU { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Quantity { get; set; }
}