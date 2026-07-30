using AutoMapper;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs.Notification;
using SmartInventory.Application.DTOs.Purchase;
using SmartInventory.Application.Exceptions;
using SmartInventory.Application.Interfaces;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;
using SmartInventory.Shared.Common;
using SmartInventory.Shared.QueryParameters;

namespace SmartInventory.Application.Services
{
    public class PurchaseService:IPurchaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<PurchaseService> _logger;
        private readonly INotificationService _notificationService;

        public PurchaseService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserService currentUser, 
            ILogger<PurchaseService> logger,
            INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
            _logger = logger;
            _notificationService = notificationService;
        }
        public async Task<PagedResult<PurchaseDto>> GetAllAsync(PurchaseQueryParameters request)
        {
            var result = await _unitOfWork.PurchaseRepository
                .GetPagedPurchasesAsync(request);
            _logger.LogInformation("Fetching purchases. Page: {Page}, Size: {Size}", request.PageNumber, request.PageSize);
            return new PagedResult<PurchaseDto>
            {
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalRecords = result.TotalRecords,

                Items = _mapper.Map<IEnumerable<PurchaseDto>>(result.Items)
            };
        }
        public async Task<PurchaseDto> CreateAsync(CreatePurchaseDto dto)
        {           
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var supplier =await ValidateSupplierAsync(dto.SupplierId);
                var purchase = new Purchase
                {
                    InvoiceNumber = $"PUR-{DateTime.UtcNow:yyyyMMddHHmmss}",
                    SupplierId = dto.SupplierId,
                    PurchaseDate = DateTime.UtcNow,
                    Status = "Completed",
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = _currentUser.UserName ?? "System"
                };

                foreach (var item in dto.Items)
                {
                    var product = await GetProductAsync(item.ProductId);
                    await IncreaseStockAsync(item.ProductId, item.Quantity);
                    purchase.PurchaseItems.Add(CreatePurchaseItem(item));
                }

                purchase.TotalAmount = CalculateTotal(dto.Items);
                _logger.LogInformation("Creating purchase for Supplier Id {SupplierId}", dto.SupplierId);
                await _unitOfWork.PurchaseRepository.AddAsync(purchase);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                await _notificationService.CreateAsync(new CreateNotificationDto
                {
                    Title = "New Purchase",
                    Message = $"Purchase from {supplier.Name} has been added.",
                    Type = "Success",
                    Url = "/purchases"
                });

                _logger.LogInformation("Purchase {InvoiceNumber} created successfully.", purchase.InvoiceNumber);

                return _mapper.Map<PurchaseDto>(purchase);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Error occurred while creating purchase.");
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<PurchaseDto?> GetByIdAsync(int id)
        {
            var purchase = await _unitOfWork.PurchaseRepository
                .GetPurchaseWithItemsAsync(id);

            if (purchase == null)
                return null;

            return _mapper.Map<PurchaseDto>(purchase);
        }

        private async Task<Supplier> ValidateSupplierAsync(int supplierId)
        {
            var supplier = await _unitOfWork.SupplierRepository.GetByIdAsync(supplierId);

            if (supplier == null)
                throw new Exception("Supplier not found.");

            return supplier;
        }
        private async Task<Product> GetProductAsync(int productId)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);

            if (product == null)
                throw new NotFoundException($"Product Id {productId} not found.");

            return product;
        }
        public async Task<bool> UpdateAsync(int id, UpdatePurchaseDto dto)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await ValidateSupplierAsync(dto.SupplierId);

                var purchase = await _unitOfWork.PurchaseRepository.GetPurchaseForUpdateAsync(id);

                if (purchase == null)
                    throw new NotFoundException("Purchase not found.");

                foreach (var item in purchase.PurchaseItems)
                {
                    await DecreaseStockAsync(item.ProductId, item.Quantity);
                }

                _unitOfWork.PurchaseItemRepository.DeleteRange(purchase.PurchaseItems);
                purchase.PurchaseItems.Clear();

                foreach (var item in dto.Items)
                {
                    await IncreaseStockAsync(item.ProductId, item.Quantity);

                    purchase.PurchaseItems.Add(CreatePurchaseItem(item));
                }

                purchase.SupplierId = dto.SupplierId;
                purchase.Status = dto.Status;
                purchase.TotalAmount = CalculateTotal(dto.Items);
                purchase.ModifiedOn = DateTime.UtcNow;
                purchase.ModifiedBy = _currentUser.UserName ?? "System";

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation(
                    "Purchase {PurchaseId} updated successfully.",
                    purchase.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating purchase {PurchaseId}", id);

                await _unitOfWork.RollbackTransactionAsync();

                throw;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var purchase = await _unitOfWork.PurchaseRepository
                .GetPurchaseForUpdateAsync(id);

            if (purchase == null)
                return false;

            _logger.LogWarning(
                "Deleting Purchase Id {Id}",
                id);

            // Rollback Product Stock
            foreach (var item in purchase.PurchaseItems)
            {
                await DecreaseStockAsync(item.ProductId, item.Quantity);
            }

            _unitOfWork.PurchaseRepository.Delete(purchase);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
        private PurchaseItem CreatePurchaseItem(CreatePurchaseItemDto item)
        {
            return new PurchaseItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.Quantity * item.UnitPrice,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = _currentUser.UserName ?? "System"
            };
        }
        private decimal CalculateTotal(IEnumerable<CreatePurchaseItemDto> items)
        {
            return items.Sum(x => x.Quantity * x.UnitPrice);
        }
        private async Task IncreaseStockAsync(int productId, int quantity)
        {
            var product = await GetProductAsync(productId);
            product.Quantity += quantity;
            _unitOfWork.ProductRepository.Update(product);
        }
        private async Task DecreaseStockAsync(int productId, int quantity)
        {
            var product = await GetProductAsync(productId);

            if (product.Quantity < quantity)
                throw new BadRequestException(
                    $"Insufficient stock for product '{product.Name}'.");

            product.Quantity -= quantity;

            _unitOfWork.ProductRepository.Update(product);
        }
    }
}
