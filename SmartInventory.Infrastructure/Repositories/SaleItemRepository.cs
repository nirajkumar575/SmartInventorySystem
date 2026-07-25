using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;
using SmartInventory.Infrastructure.Data;

namespace SmartInventory.Infrastructure.Repositories;

public class SaleItemRepository
    : GenericRepository<SaleItem>, ISaleItemRepository
{
    public SaleItemRepository(AppDbContext context)
        : base(context)
    {
    }
}