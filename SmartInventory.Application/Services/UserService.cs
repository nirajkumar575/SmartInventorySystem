using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs.User;
using SmartInventory.Application.Interfaces;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UserService> logger,
        UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        _logger.LogInformation("Fetching all users.");
        var users = await _userManager.Users.ToListAsync();
        var userDtos = new List<UserDto>();
        foreach (var user in users)
        {
            var dto = _mapper.Map<UserDto>(user);
            var roles = await _userManager.GetRolesAsync(user);
            dto.Role = roles.FirstOrDefault();
            userDtos.Add(dto);
        }

        return userDtos;
    }

    public async Task<UserDto?> GetByIdAsync(string id)
    {
        _logger.LogInformation("Fetching user {UserId}", id);
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            return null;

        var userDto = _mapper.Map<UserDto>(user);
        var roles = await _userManager.GetRolesAsync(user);
        userDto.Role = roles.FirstOrDefault();
        return userDto;
    }

    public async Task<bool> UpdateAsync(string id, UpdateUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            return false;

        _mapper.Map(dto, user);
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return false;

        if (!string.IsNullOrEmpty(dto.Role))
        {
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
            }

            await _userManager.AddToRoleAsync(user, dto.Role);
        }

        _logger.LogInformation("User {UserId} updated successfully.", id);

        return true;
    }


    public async Task<bool> DeleteAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            return false;

        _logger.LogWarning("Deleting user {UserId}", id);

        user.IsActive = false;

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }
}