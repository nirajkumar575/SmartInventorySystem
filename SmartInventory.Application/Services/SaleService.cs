using AutoMapper;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs.Notification;
using SmartInventory.Application.DTOs.Sale;
using SmartInventory.Application.Exceptions;
using SmartInventory.Application.Interfaces;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;
using SmartInventory.Shared.Common;
using SmartInventory.Shared.QueryParameters;

namespace SmartInventory.Application.Services;

public class SaleService : ISaleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly ILogger<SaleService> _logger;
    private readonly INotificationService _notificationService;

    public SaleService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUser,
        ILogger<SaleService> logger,
        INotificationService notificationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUser = currentUser;
        _logger = logger;
        _notificationService = notificationService;
    }

    public async Task<PagedResult<SaleDto>> GetAllAsync(SaleQueryParameters request)
    {
        var result = await _unitOfWork.SaleRepository
            .GetPagedSalesAsync(request);

        _logger.LogInformation(
            "Fetching sales. Page:{Page} Size:{Size}",
            request.PageNumber,
            request.PageSize);

        return new PagedResult<SaleDto>
        {
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords,
            Items = _mapper.Map<IEnumerable<SaleDto>>(result.Items)
        };
    }

    public async Task<SaleDto?> GetByIdAsync(int id)
    {
        var sale = await _unitOfWork.SaleRepository
            .GetSaleWithItemsAsync(id);

        if (sale == null)
            return null;

        return _mapper.Map<SaleDto>(sale);
    }

    public async Task<SaleDto> CreateAsync(CreateSaleDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            await ValidateCustomerAsync(dto.CustomerId);

            var sale = new Sale
            {
                InvoiceNumber = $"SAL-{DateTime.UtcNow:yyyyMMddHHmmss}",
                CustomerId = dto.CustomerId,
                SaleDate = DateTime.UtcNow,
                Status = "Completed",
                CreatedOn = DateTime.UtcNow,
                CreatedBy = _currentUser.UserName ?? "System"
            };

            foreach (var item in dto.Items)
            {
                var product = await GetProductAsync(item.ProductId);

                if (product.Quantity < item.Quantity)
                    throw new BadRequestException(
                        $"Insufficient stock for {product.Name}");

                product.Quantity -= item.Quantity;

                _unitOfWork.ProductRepository.Update(product);

                sale.SaleItems.Add(CreateSaleItem(item));
            }

            sale.TotalAmount = CalculateTotal(dto.Items);

            await _unitOfWork.SaleRepository.AddAsync(sale);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();
            await _notificationService.CreateAsync(new CreateNotificationDto
            {
                Title = "New Sale",
                Message = $"Sale Invoice #{sale.InvoiceNumber} created.",
                Type = "Info",
                Url = "/sales"
            });
            _logger.LogInformation(
                "Sale created successfully. Invoice:{Invoice}",
                sale.InvoiceNumber);

            return _mapper.Map<SaleDto>(sale);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> UpdateAsync(int id, UpdateSaleDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            await ValidateCustomerAsync(dto.CustomerId);

            var sale = await _unitOfWork.SaleRepository
                .GetSaleForUpdateAsync(id);

            if (sale == null)
                throw new NotFoundException("Sale not found.");

            // Restore previous stock
            foreach (var item in sale.SaleItems)
            {
                var product = await GetProductAsync(item.ProductId);

                product.Quantity += item.Quantity;

                _unitOfWork.ProductRepository.Update(product);
            }

            // Remove old items
            _unitOfWork.SaleItemRepository.DeleteRange(sale.SaleItems);

            sale.SaleItems.Clear();

            // Update Header
            sale.CustomerId = dto.CustomerId;
            sale.Status = dto.Status;
            sale.ModifiedOn = DateTime.UtcNow;
            sale.ModifiedBy = _currentUser.UserName ?? "System";

            decimal total = 0;

            // Add new items
            foreach (var item in dto.Items)
            {
                var product = await GetProductAsync(item.ProductId);

                if (product.Quantity < item.Quantity)
                    throw new BadRequestException(
                        $"Insufficient stock for product {product.Name}");

                product.Quantity -= item.Quantity;

                _unitOfWork.ProductRepository.Update(product);

                sale.SaleItems.Add(new SaleItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.Quantity * item.UnitPrice,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = _currentUser.UserName ?? "System"
                });

                total += item.Quantity * item.UnitPrice;
            }

            sale.TotalAmount = total;

            _unitOfWork.SaleRepository.Update(sale);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation(
                "Sale {Id} updated successfully.",
                id);

            return true;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var sale = await _unitOfWork.SaleRepository
                .GetSaleForUpdateAsync(id);

            if (sale == null)
                throw new NotFoundException("Sale not found.");

            foreach (var item in sale.SaleItems)
            {
                var product = await GetProductAsync(item.ProductId);

                product.Quantity += item.Quantity;

                product.ModifiedOn = DateTime.UtcNow;
                product.ModifiedBy = _currentUser.UserName ?? "System";

                _unitOfWork.ProductRepository.Update(product);
            }

            _unitOfWork.SaleItemRepository.DeleteRange(sale.SaleItems);

            sale.ModifiedOn = DateTime.UtcNow;
            sale.ModifiedBy = _currentUser.UserName ?? "System";

            _unitOfWork.SaleRepository.Delete(sale);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();

            _logger.LogInformation(
                "Sale {SaleId} deleted successfully.",
                id);

            return true;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    private async Task ValidateCustomerAsync(int customerId)
    {
        var customer = await _unitOfWork.CustomerRepository
            .GetByIdAsync(customerId);

        if (customer == null)
            throw new NotFoundException(
                $"Customer Id {customerId} not found.");
    }

    private async Task<Product> GetProductAsync(int productId)
    {
        var product = await _unitOfWork.ProductRepository
            .GetByIdAsync(productId);

        if (product == null)
            throw new NotFoundException(
                $"Product Id {productId} not found.");

        return product;
    }

    private SaleItem CreateSaleItem(CreateSaleItemDto item)
    {
        return new SaleItem
        {
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice,
            TotalPrice = item.Quantity * item.UnitPrice,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = _currentUser.UserName ?? "System"
        };
    }

    private decimal CalculateTotal(IEnumerable<CreateSaleItemDto> items)
    {
        return items.Sum(x => x.Quantity * x.UnitPrice);
    }
}