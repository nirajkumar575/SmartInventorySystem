using Microsoft.AspNetCore.Mvc;
using SmartInventory.Application.DTOs.Settings;
using SmartInventory.Application.Interfaces;

namespace SmartInventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingsController : ControllerBase
{
    private readonly IAppSettingService _settingService;

    public SettingsController(IAppSettingService settingService)
    {
        _settingService = settingService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _settingService.GetAsync();

        if (result == null)
            return NotFound(new
            {
                message = "Settings not found."
            });

        return Ok(result);
    }
    [HttpPut]
    public async Task<IActionResult> Save(UpdateAppSettingDto dto)
    {
        var result = await _settingService.SaveAsync(dto);

        if (!result)
            return BadRequest(new
            {
                message = "Unable to save settings."
            });

        return Ok(new
        {
            message = "Settings saved successfully."
        });
    }
}