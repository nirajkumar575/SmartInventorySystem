namespace SmartInventory.Application.DTOs.Sale;

public class UpdateSaleDto
{
    public int CustomerId { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<CreateSaleItemDto> Items { get; set; } = new();
}