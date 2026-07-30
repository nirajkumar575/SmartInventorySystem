using AutoMapper;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs.Settings;
using SmartInventory.Application.Interfaces;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Services;

public class AppSettingService : IAppSettingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AppSettingService> _logger;
    private readonly ICurrentUserService _currentUser;

    public AppSettingService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<AppSettingService> logger,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _currentUser = currentUserService;
    }

    public async Task<AppSettingDto?> GetAsync()
    {
        _logger.LogInformation("Fetching application settings.");

        var setting = await _unitOfWork.AppSettingRepository.GetSettingAsync();

        if (setting == null)
            return null;

        return _mapper.Map<AppSettingDto>(setting);
    }

    public async Task<bool> SaveAsync(UpdateAppSettingDto dto)
    {
        var setting = await _unitOfWork.AppSettingRepository.GetSettingAsync();

        if (setting == null)
        {
            _logger.LogInformation("Creating application settings.");

            setting = _mapper.Map<AppSetting>(dto);

            setting.CreatedOn = DateTime.UtcNow;
            setting.CreatedBy = _currentUser.UserName ?? "System";

            await _unitOfWork.AppSettingRepository.AddAsync(setting);
        }
        else
        {
            _logger.LogInformation("Updating application settings.");

            _mapper.Map(dto, setting);

            setting.ModifiedOn = DateTime.UtcNow;
            setting.ModifiedBy = _currentUser.UserName ?? "System";

            _unitOfWork.AppSettingRepository.Update(setting);
        }

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}