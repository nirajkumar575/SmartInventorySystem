namespace SmartInventory.Application.DTOs.Purchase
{
    public class CreatePurchaseItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
