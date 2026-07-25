using SmartInventory.Application.DTOs.Category;
using SmartInventory.Shared.Common;

namespace SmartInventory.Application.Interfaces;

public interface ICategoryService
{
    Task<PagedResult<CategoryDto>> GetPagedAsync(CategoryQueryParameters request);
    Task<CategoryDto?> GetByIdAsync(int id);
    Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
    Task<bool> UpdateAsync(int id, UpdateCategoryDto dto);
    Task<bool> DeleteAsync(int id);
}