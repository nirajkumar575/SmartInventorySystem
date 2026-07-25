namespace SmartInventory.Domain.Common;

public class TopSellingProduct
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
}