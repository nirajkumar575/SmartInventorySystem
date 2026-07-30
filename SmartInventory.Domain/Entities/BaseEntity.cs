namespace SmartInventory.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = string.Empty;

    public DateTime? ModifiedOn { get; set; }
    public string ModifiedBy { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
    public DateTime DeletedOn { get; set; }
    public string DeletedBy { get; set; }=string.Empty;
}