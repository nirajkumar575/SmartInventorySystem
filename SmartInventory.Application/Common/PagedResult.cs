namespace SmartInventory.Application.Common;

public class PagedResult<T>
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalRecords { get; set; }

    public int TotalPages =>
        (int)Math.Ceiling((double)TotalRecords / PageSize);

    public IEnumerable<T> Items { get; set; } = new List<T>();
}