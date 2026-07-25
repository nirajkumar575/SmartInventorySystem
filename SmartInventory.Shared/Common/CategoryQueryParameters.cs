namespace SmartInventory.Shared.Common;

public class CategoryQueryParameters : PaginationRequest
{
    public string? Search { get; set; }
    public bool? IsActive { get; set; }
}