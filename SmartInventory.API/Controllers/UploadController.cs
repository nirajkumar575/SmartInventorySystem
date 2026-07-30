using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.Interfaces;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class UploadController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public UploadController(
        IWebHostEnvironment environment,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _environment = environment;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    [HttpPost("logo")]
    public async Task<IActionResult> UploadLogo(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new
            {
                message = "No file selected."
            });

        var webRootPath = _environment.WebRootPath;

        if (string.IsNullOrWhiteSpace(webRootPath))
        {
            webRootPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot");

            Directory.CreateDirectory(webRootPath);
        }

        // Create uploads/logo folder
        var uploadFolder = Path.Combine(
            webRootPath,
            "uploads",
            "logo");

        Directory.CreateDirectory(uploadFolder);

        // Generate unique filename
        var fileName =
            $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        var filePath = Path.Combine(uploadFolder, fileName);

        // Save file
        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Public URL
        var logoUrl =
            $"{Request.Scheme}://{Request.Host}/uploads/logo/{fileName}";

        // Get existing settings
        var setting =
            await _unitOfWork.AppSettingRepository.GetSettingAsync();

        if (setting == null)
        {
            setting = new AppSetting
            {
                CompanyLogo = logoUrl,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = _currentUserService.UserName ?? "System"
            };

            await _unitOfWork
                .AppSettingRepository
                .AddAsync(setting);
        }
        else
        {
            setting.CompanyLogo = logoUrl;
            setting.ModifiedOn = DateTime.UtcNow;
            setting.ModifiedBy = _currentUserService.UserName ?? "System";

            _unitOfWork
                .AppSettingRepository
                .Update(setting);
        }

        await _unitOfWork.SaveChangesAsync();

        return Ok(new
        {
            logoUrl
        });
    }
}