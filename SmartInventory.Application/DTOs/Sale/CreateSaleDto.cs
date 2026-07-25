namespace SmartInventory.Application.DTOs.Sale;

public class CreateSaleDto
{
    public int CustomerId { get; set; }
    public List<CreateSaleItemDto> Items { get; set; } = new();
}