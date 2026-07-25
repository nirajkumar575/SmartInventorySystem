using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;
using SmartInventory.Infrastructure.Data;

namespace SmartInventory.Infrastructure.Repositories;

public class PurchaseItemRepository
    : GenericRepository<PurchaseItem>, IPurchaseItemRepository
{
    public PurchaseItemRepository(AppDbContext context)
        : base(context)
    {
    }
}