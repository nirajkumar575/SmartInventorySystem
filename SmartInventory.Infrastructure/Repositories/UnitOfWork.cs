using Microsoft.EntityFrameworkCore.Storage;
using SmartInventory.Domain.Interfaces;
using SmartInventory.Infrastructure.Data;

namespace SmartInventory.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    public IProductRepository ProductRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public ISupplierRepository SupplierRepository { get; }
    public IPurchaseRepository PurchaseRepository { get; }
    public ICustomerRepository CustomerRepository { get; }
    public ISaleRepository SaleRepository { get; }
    public ISaleItemRepository SaleItemRepository { get; }
    public IPurchaseItemRepository PurchaseItemRepository { get; }
    public IStockAdjustmentRepository StockAdjustmentRepository { get; }
    public IAppSettingRepository AppSettingRepository { get; }
    public IAuditLogRepository AuditLogRepository { get; }
    public INotificationRepository Notifications { get; }



    public UnitOfWork(AppDbContext context,
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        ISupplierRepository supplierRepository,
        IPurchaseRepository purchaseRepository,
        ICustomerRepository customerRepository,
        ISaleRepository saleRepository,
        ISaleItemRepository saleItemRepository,
        IPurchaseItemRepository purchaseItemRepository,
        IStockAdjustmentRepository stockAdjustmentRepository,
        IAppSettingRepository appSettingRepository,
        IAuditLogRepository auditLogRepository,
        INotificationRepository notifications)
    {
        _context = context;
        ProductRepository = productRepository;
        CategoryRepository = categoryRepository;
        SupplierRepository = supplierRepository;
        PurchaseRepository = purchaseRepository;
        CustomerRepository = customerRepository;
        SaleRepository = saleRepository;
        SaleItemRepository = saleItemRepository;
        PurchaseItemRepository = purchaseItemRepository;
        StockAdjustmentRepository = stockAdjustmentRepository;
        AppSettingRepository = appSettingRepository;
        AuditLogRepository = auditLogRepository;
        Notifications = notifications;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }


    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}