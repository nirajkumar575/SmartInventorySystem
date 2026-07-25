using AutoMapper;
using SmartInventory.Application.DTOs.Supplier;
using SmartInventory.Application.Interfaces;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public SupplierService(IUnitOfWork unitOfWork,IMapper mapper,ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<SupplierDto>> GetAllAsync()
    {
        var suppliers = await _unitOfWork.SupplierRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
    }

    public async Task<SupplierDto?> GetByIdAsync(int id)
    {
        var supplier = await _unitOfWork.SupplierRepository.GetByIdAsync(id);

        if (supplier == null)
            return null;
        return _mapper.Map<SupplierDto>(supplier);
    }

    public async Task<SupplierDto> CreateAsync(CreateSupplierDto dto)
    {
        if (await _unitOfWork.SupplierRepository.GetByEmailAsync(dto.Email) != null)
            throw new Exception("Supplier email already exists.");

        var supplier = _mapper.Map<Supplier>(dto);

        supplier.IsActive = true;
        supplier.CreatedOn = DateTime.UtcNow;
        supplier.CreatedBy = _currentUserService.UserName ?? "System";

        await _unitOfWork.SupplierRepository.AddAsync(supplier);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<SupplierDto>(supplier);
    }

    public async Task<bool> UpdateAsync(int id, UpdateSupplierDto dto)
    {
        var supplier = await _unitOfWork.SupplierRepository.GetByIdAsync(id);

        if (supplier == null)
            return false;

        _mapper.Map(dto, supplier);

        supplier.ModifiedOn = DateTime.UtcNow;
        supplier.ModifiedBy = _currentUserService.UserName ?? "System";

        _unitOfWork.SupplierRepository.Update(supplier);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var supplier = await _unitOfWork.SupplierRepository.GetByIdAsync(id);

        if (supplier == null)
            return false;

        supplier.IsDeleted = true;
        supplier.DeletedOn = DateTime.UtcNow;
        supplier.DeletedBy = _currentUserService.UserName ?? "System";

        _unitOfWork.SupplierRepository.Update(supplier);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}