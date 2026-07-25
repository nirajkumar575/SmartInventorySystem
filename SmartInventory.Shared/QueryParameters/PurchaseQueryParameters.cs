using SmartInventory.Shared.Common;

namespace SmartInventory.Shared.QueryParameters;

public class PurchaseQueryParameters : PaginationRequest
{
    public string? Search { get; set; }
    public int? SupplierId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? SortBy { get; set; } = "date";
    public string? SortOrder { get; set; } = "desc";
}