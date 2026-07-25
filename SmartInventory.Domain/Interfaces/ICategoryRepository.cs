using SmartInventory.Domain.Entities;
using SmartInventory.Shared.Common;

namespace SmartInventory.Domain.Interfaces;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<Category?> GetByNameAsync(string name);
    Task<PagedResult<Category>> GetPagedCategoriesAsync(CategoryQueryParameters request);
}