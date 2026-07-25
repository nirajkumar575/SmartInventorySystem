using AutoMapper;
using SmartInventory.Application.DTOs.Reports;
using SmartInventory.Application.Interfaces;
using SmartInventory.Domain.Interfaces;
using SmartInventory.Shared.QueryParameters;

namespace SmartInventory.Application.Services;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SalesReportDto>> GetSalesReportAsync(ReportQueryParameters request)
    {
        var sales = await _unitOfWork.SaleRepository.GetSalesReportAsync(request);

        return _mapper.Map<IEnumerable<SalesReportDto>>(sales);
    }

    public async Task<IEnumerable<PurchaseReportDto>> GetPurchaseReportAsync(ReportQueryParameters request)
    {
        var purchases = await _unitOfWork.PurchaseRepository.GetPurchaseReportAsync(request);

        return _mapper.Map<IEnumerable<PurchaseReportDto>>(purchases);
    }

    public async Task<IEnumerable<StockReportDto>> GetStockReportAsync()
    {
        var products = await _unitOfWork.ProductRepository.GetStockReportAsync();

        var result = _mapper.Map<List<StockReportDto>>(products);

        foreach (var item in result)
        {
            item.IsLowStock = item.Quantity < 10;
        }

        return result;
    }

    public async Task<ProfitReportDto> GetProfitReportAsync(ReportQueryParameters request)
    {
        var sales = await _unitOfWork.SaleRepository.GetSalesReportAsync(request);
        var purchases = await _unitOfWork.PurchaseRepository.GetPurchaseReportAsync(request);

        return new ProfitReportDto
        {
            TotalSales = sales.Sum(x => x.TotalAmount),
            TotalPurchase = purchases.Sum(x => x.TotalAmount),
            Profit = sales.Sum(x => x.TotalAmount) - purchases.Sum(x => x.TotalAmount)
        };
    }
}