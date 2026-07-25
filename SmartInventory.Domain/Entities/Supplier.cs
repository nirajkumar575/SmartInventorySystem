using SmartInventory.Domain.Common;

namespace SmartInventory.Domain.Entities;

public class Supplier : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? GSTNumber { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}