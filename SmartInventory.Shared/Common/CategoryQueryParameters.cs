namespace SmartInventory.Shared.Common;

public class CategoryQueryParameters : PaginationRequest
{
    public bool? IsActive { get; set; }
}