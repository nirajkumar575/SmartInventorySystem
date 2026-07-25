namespace SmartInventory.Application.DTOs.Purchase
{
    public class PurchaseDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<PurchaseItemDto> Items { get; set; } = new();
    }
}
