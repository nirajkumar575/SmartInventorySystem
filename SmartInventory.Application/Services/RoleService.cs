using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs.Role;
using SmartInventory.Application.Interfaces;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<RoleService> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditLogService _auditLogService;

    public RoleService(
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager,
        ILogger<RoleService> logger,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IAuditLogService auditLogService)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _auditLogService = auditLogService;
    }

    public async Task<List<RoleDto>> GetAllAsync()
    {
        var roles = _roleManager.Roles.ToList();

        return _mapper.Map<List<RoleDto>>(roles);
    }

    public async Task<bool> CreateAsync(CreateRoleDto dto)
    {
        if (await _roleManager.RoleExistsAsync(dto.Name))
            return false;

        var result = await _roleManager.CreateAsync(
            new IdentityRole(dto.Name));

        if (!result.Succeeded)
            return false;

        await _auditLogService.AddAsync(
            "Role",
            "Create",
            $"Role '{dto.Name}' created.");

        _logger.LogInformation("Role {Role} created.", dto.Name);

        return true;
    }

    public async Task<bool> DeleteAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);

        if (role == null)
            return false;

        var result = await _roleManager.DeleteAsync(role);

        if (!result.Succeeded)
            return false;

        await _auditLogService.AddAsync(
            "Role",
            "Delete",
            $"Role '{roleName}' deleted.");

        return true;
    }

    public async Task<bool> AssignRoleAsync(UserRoleDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId);

        if (user == null)
            return false;

        var currentRoles = await _userManager.GetRolesAsync(user);

        if (currentRoles.Any())
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

        var result = await _userManager.AddToRoleAsync(
            user,
            dto.RoleName);

        if (!result.Succeeded)
            return false;

        await _auditLogService.AddAsync(
            "Role",
            "Assign",
            $"Role '{dto.RoleName}' assigned to '{user.UserName}'.");

        return true;
    }
}