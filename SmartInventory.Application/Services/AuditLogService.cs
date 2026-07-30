using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs.Audit;
using SmartInventory.Application.Interfaces;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AuditLogService> _logger;
    private readonly ICurrentUserService _currentUser;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditLogService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<AuditLogService> logger,
        ICurrentUserService currentUser,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _currentUser = currentUser;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task AddAsync(
        string module,
        string action,
        string description)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var audit = new AuditLog
        {
            UserName = _currentUser.UserName ?? "System",
            Module = module,
            Action = action,
            Description = description,
            IpAddress = httpContext?.Connection.RemoteIpAddress?.ToString(),
            Browser = httpContext?.Request.Headers.UserAgent.ToString(),
            ActionDate = DateTime.UtcNow,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = _currentUser.UserName ?? "System"
        };

        await _unitOfWork.AuditLogRepository.AddAsync(audit);

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation(
            "Audit Log Added: {Module} {Action}",
            module,
            action);
    }

    public async Task<List<AuditLogDto>> GetAllAsync()
    {
        var logs = await _unitOfWork
            .AuditLogRepository
            .GetAllAsync();

        return _mapper.Map<List<AuditLogDto>>(logs);
    }
}