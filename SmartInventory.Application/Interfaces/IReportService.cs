using SmartInventory.Application.DTOs.Reports;
using SmartInventory.Shared.QueryParameters;

namespace SmartInventory.Application.Interfaces;

public interface IReportService
{
    Task<IEnumerable<SalesReportDto>> GetSalesReportAsync(ReportQueryParameters request);

    Task<IEnumerable<PurchaseReportDto>> GetPurchaseReportAsync(ReportQueryParameters request);
    Task<IEnumerable<StockReportDto>> GetStockReportAsync();
    Task<ProfitReportDto> GetProfitReportAsync(ReportQueryParameters request);
}