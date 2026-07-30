using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.DTOs.Role;
using SmartInventory.Application.Interfaces;

namespace SmartInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _roleService.GetAllAsync();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRoleDto dto)
    {
        var result = await _roleService.CreateAsync(dto);

        if (!result)
            return BadRequest(new
            {
                message = "Role already exists."
            });

        return Ok(new
        {
            message = "Role created successfully."
        });
    }

    [HttpDelete("{roleName}")]
    public async Task<IActionResult> Delete(string roleName)
    {
        var result = await _roleService.DeleteAsync(roleName);

        if (!result)
            return NotFound(new
            {
                message = "Role not found."
            });

        return Ok(new
        {
            message = "Role deleted successfully."
        });
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignRole(UserRoleDto dto)
    {
        var result = await _roleService.AssignRoleAsync(dto);

        if (!result)
            return BadRequest(new
            {
                message = "Unable to assign role."
            });

        return Ok(new
        {
            message = "Role assigned successfully."
        });
    }
}