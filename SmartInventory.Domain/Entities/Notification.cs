namespace SmartInventory.Domain.Entities;

public class Notification : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public string Type { get; set; } = "Info";
    public bool IsRead { get; set; } = false;
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }
    public string? Url { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}