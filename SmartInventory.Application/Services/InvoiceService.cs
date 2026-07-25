using AutoMapper;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs.Invoice;
using SmartInventory.Application.Interfaces;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<InvoiceService> _logger;

    public InvoiceService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<InvoiceService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<InvoiceDto?> GetInvoiceAsync(int saleId)
    {
        var sale = await _unitOfWork.SaleRepository.GetInvoiceAsync(saleId);

        if (sale == null)
            return null;

        _logger.LogInformation("Invoice generated for SaleId {SaleId}", saleId);

        return _mapper.Map<InvoiceDto>(sale);
    }
}