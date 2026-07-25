namespace SmartInventory.Application.DTOs.Purchase;

public class UpdatePurchaseDto
{
    public int SupplierId { get; set; }
    public string Status { get; set; } = "Completed";
    public List<CreatePurchaseItemDto> Items { get; set; } = new();
}