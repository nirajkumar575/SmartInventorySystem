using SmartInventory.Domain.Common;
using SmartInventory.Domain.Entities;
using SmartInventory.Shared.Common;
using SmartInventory.Shared.QueryParameters;

namespace SmartInventory.Domain.Interfaces
{
    public interface IPurchaseRepository : IGenericRepository<Purchase>
    {
        Task<Purchase?> GetPurchaseWithItemsAsync(int id);
        Task<PagedResult<Purchase>> GetPagedPurchasesAsync(PurchaseQueryParameters request);
        Task<Purchase?> GetPurchaseForUpdateAsync(int id);
        Task<decimal> GetTotalPurchaseAmountAsync();
        Task<decimal> GetTodayPurchasesAsync();
        Task<IEnumerable<Purchase>> GetPurchaseReportAsync(ReportQueryParameters request);

        Task<IEnumerable<Purchase>> GetRecentPurchasesAsync(int count);
        Task<IEnumerable<Purchase>> GetLast7DaysPurchasesAsync();
        Task<IEnumerable<DashboardChartData>> GetLast7DaysPurchaseChartAsync();
    }
}
