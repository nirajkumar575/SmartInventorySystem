using System.Linq.Expressions;
using SmartInventory.Shared.Common;

namespace SmartInventory.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{    
    Task<IEnumerable<T>> GetAllAsync();

    Task<T?> GetByIdAsync(int id);

    Task AddAsync(T entity);

    void Update(T entity);

    void Delete(T entity);

    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    Task<PagedResult<T>> GetPagedAsync(PaginationRequest request);
    Task SaveChangesAsync();
}