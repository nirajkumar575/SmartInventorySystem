using SmartInventory.Shared.Common;

namespace SmartInventory.Application.QueryParameters;

public class ProductQueryParameters : PaginationRequest
{
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}