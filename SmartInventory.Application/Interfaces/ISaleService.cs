using SmartInventory.Application.DTOs.Sale;
using SmartInventory.Shared.Common;
using SmartInventory.Shared.QueryParameters;

namespace SmartInventory.Application.Interfaces;

public interface ISaleService
{
    Task<PagedResult<SaleDto>> GetAllAsync(SaleQueryParameters request);
    Task<SaleDto?> GetByIdAsync(int id);
    Task<SaleDto> CreateAsync(CreateSaleDto dto);
    Task<bool> UpdateAsync(int id, UpdateSaleDto dto);
    Task<bool> DeleteAsync(int id);
}