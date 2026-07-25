namespace SmartInventory.Application.DTOs.Purchase
{
    public class CreatePurchaseDto
    {
        public int SupplierId { get; set; }
        public List<CreatePurchaseItemDto> Items { get; set; } = new();
    }
}
