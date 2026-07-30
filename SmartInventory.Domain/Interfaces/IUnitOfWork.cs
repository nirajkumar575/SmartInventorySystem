namespace SmartInventory.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IProductRepository ProductRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    ISupplierRepository SupplierRepository { get; }
    IPurchaseRepository PurchaseRepository { get; }
    ICustomerRepository CustomerRepository { get; }
    ISaleRepository SaleRepository { get; }
    ISaleItemRepository SaleItemRepository { get; }
    IPurchaseItemRepository PurchaseItemRepository { get; }
    IStockAdjustmentRepository StockAdjustmentRepository { get; }
    IAppSettingRepository AppSettingRepository { get; }
    IAuditLogRepository AuditLogRepository { get; }
    INotificationRepository Notifications { get; }


    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}