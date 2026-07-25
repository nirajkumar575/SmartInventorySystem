using SmartInventory.Application.DTOs.Supplier;

namespace SmartInventory.Application.Interfaces;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDto>> GetAllAsync();
    Task<SupplierDto?> GetByIdAsync(int id);
    Task<SupplierDto> CreateAsync(CreateSupplierDto dto);
    Task<bool> UpdateAsync(int id, UpdateSupplierDto dto);
    Task<bool> DeleteAsync(int id);
}