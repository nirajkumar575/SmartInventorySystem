using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.DTOs.Auth;
using SmartInventory.Application.DTOs.Notification;
using SmartInventory.Application.Interfaces;
using SmartInventory.Application.Services;
using SmartInventory.Domain.Entities;

namespace SmartInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly INotificationService _notificationService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        IJwtTokenService jwtTokenService,
        INotificationService notificationService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
        _notificationService = notificationService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingUser = await _userManager.FindByEmailAsync(model.Email);

        if (existingUser != null)
            return BadRequest(new { message = "Email already exists." });

        var user = new ApplicationUser
        {
            FullName = model.FullName,
            UserName = model.UserName,
            Email = model.Email,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        if (model.Password != model.ConfirmPassword)
        {
            return BadRequest(new
            {
                message = "Password and Confirm Password do not match."
            });
        }

        await _userManager.AddToRoleAsync(user, "Employee");
        await _notificationService.CreateAsync(new CreateNotificationDto
        {
            Title = "New User",
            Message = $"{user.FullName} has been added.",
            Type = "Info",
            Url = "/users"
        });

        return Ok(new
        {
            message = "User registered successfully."
        });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        if (!user.IsActive)
        {
            return Unauthorized(new
            {
                message = "Your account is inactive. Please contact the administrator."
            });
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);

        if (!isPasswordValid)
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });

        var response = await _jwtTokenService.GenerateTokensAsync(user);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
    RefreshTokenRequestDto request)
    {
        var result = await _jwtTokenService.RefreshTokenAsync(request.RefreshToken);

        if (result == null)
            return Unauthorized(new
            {
                message = "Invalid refresh token."
            });

        return Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
    RefreshTokenRequestDto request)
    {
        await _jwtTokenService.RevokeRefreshTokenAsync(request.RefreshToken);

        return Ok(new
        {
            message = "Logged out successfully."
        });
    }
}