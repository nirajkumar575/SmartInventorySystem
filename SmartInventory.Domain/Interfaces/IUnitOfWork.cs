namespace SmartInventory.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IProductRepository ProductRepository { get; }
    Task<int> SaveChangesAsync();
}