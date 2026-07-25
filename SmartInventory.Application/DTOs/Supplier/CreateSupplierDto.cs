using System.ComponentModel.DataAnnotations;

namespace SmartInventory.Application.DTOs.Supplier;

public class CreateSupplierDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? GSTNumber { get; set; }
}