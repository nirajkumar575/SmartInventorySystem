using SmartInventory.Shared.Common;

namespace SmartInventory.Shared.QueryParameters;

public class SaleQueryParameters : PaginationRequest
{
    public string? Search { get; set; }
    public int? CustomerId { get; set; }
    public string? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string SortBy { get; set; } = "SaleDate";
    public bool Descending { get; set; } = true;
}