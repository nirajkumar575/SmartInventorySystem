namespace SmartInventory.Application.DTOs.Role;

using System.ComponentModel.DataAnnotations;

public class CreateRoleDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
}