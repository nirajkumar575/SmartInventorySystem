using SmartInventory.Domain.Entities;
using SmartInventory.Shared.Common;
using SmartInventory.Shared.QueryParameters;

namespace SmartInventory.Domain.Interfaces;

public interface ISaleRepository : IGenericRepository<Sale>
{
    Task<Sale?> GetSaleWithItemsAsync(int id);
    Task<Sale?> GetSaleForUpdateAsync(int id);
    Task<PagedResult<Sale>> GetPagedSalesAsync(SaleQueryParameters request);
    Task<decimal> GetTotalRevenueAsync();
    Task<decimal> GetTodaySalesAsync();
    Task<IEnumerable<Sale>> GetSalesReportAsync(ReportQueryParameters request);
}