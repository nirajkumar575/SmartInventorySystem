using Microsoft.EntityFrameworkCore;
using SmartInventory.Domain.Common;
using SmartInventory.Domain.Interfaces;
using SmartInventory.Infrastructure.Data;
using SmartInventory.Shared.Common;
using System.Linq.Expressions;

namespace SmartInventory.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public virtual async Task<T?> GetByIdAsync(int id)
        => await _dbSet.FindAsync(id);

    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public void Update(T entity)
        => _dbSet.Update(entity);

    public void Delete(T entity)
    {
        if (entity is BaseEntity baseEntity)
        {
            baseEntity.IsDeleted = true;
            baseEntity.ModifiedOn = DateTime.UtcNow;

            _dbSet.Update(entity);
            return;
        }

        _dbSet.Remove(entity);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.AnyAsync(predicate);

    public async Task<PagedResult<T>> GetPagedAsync(
    PaginationRequest request)
    {
        var query = _dbSet.AsQueryable();
        var totalRecords = await query.CountAsync();
        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();
        return new PagedResult<T>
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalRecords = totalRecords,
            Items = items
        };
    }
    public void DeleteRange(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            Delete(entity);
        }
    }

    public async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.CountAsync(predicate);
    }

    public async Task<decimal> SumAsync(Expression<Func<T, decimal>> selector)
    {
        return await _dbSet
            .Select(selector)
            .DefaultIfEmpty(0)
            .SumAsync();
    }

    public async Task<decimal> SumAsync(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, decimal>> selector)
    {
        return await _dbSet
            .Where(predicate)
            .Select(selector)
            .DefaultIfEmpty(0)
            .SumAsync();
    }
}