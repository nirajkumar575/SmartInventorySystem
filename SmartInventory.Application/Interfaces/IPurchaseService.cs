using SmartInventory.Application.DTOs.Purchase;
using SmartInventory.Shared.Common;
using SmartInventory.Shared.QueryParameters;

namespace SmartInventory.Application.Interfaces;

public interface IPurchaseService
{
    Task<PagedResult<PurchaseDto>> GetAllAsync(PurchaseQueryParameters request);
    Task<PurchaseDto> CreateAsync(CreatePurchaseDto dto);
    Task<PurchaseDto?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(int id, UpdatePurchaseDto dto);
    Task<bool> DeleteAsync(int id);
}