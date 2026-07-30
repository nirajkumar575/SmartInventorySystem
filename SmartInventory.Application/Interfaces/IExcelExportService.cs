using SmartInventory.Application.DTOs.Reports;

public interface IExcelExportService
{
    byte[] GenerateSalesReportExcel(IEnumerable<SalesReportDto> data);
    byte[] GeneratePurchaseReportExcel(IEnumerable<PurchaseReportDto> data);
    byte[] GenerateStockReportExcel(IEnumerable<StockReportDto> data);
}