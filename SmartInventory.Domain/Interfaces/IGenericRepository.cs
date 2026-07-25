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
    void DeleteRange(IEnumerable<T> entities);

    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    Task<decimal> SumAsync(Expression<Func<T, decimal>> selector);
    Task<decimal> SumAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, decimal>> selector);
}