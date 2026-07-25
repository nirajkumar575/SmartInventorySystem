using AutoMapper;
using SmartInventory.Application.DTOs.Category;
using SmartInventory.Application.Interfaces;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;
using SmartInventory.Shared.Common;

namespace SmartInventory.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public CategoryService(IUnitOfWork unitOfWork,IMapper mapper, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<PagedResult<CategoryDto>> GetPagedAsync(CategoryQueryParameters request)
    {
        var result = await _unitOfWork.CategoryRepository.GetPagedCategoriesAsync(request);

        return new PagedResult<CategoryDto>
        {
            Items = _mapper.Map<List<CategoryDto>>(result.Items),
            TotalRecords = result.TotalRecords,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

        if (category == null)
            return null;

        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        var existing = await _unitOfWork.CategoryRepository.GetByNameAsync(dto.Name);

        if (existing != null)
            throw new Exception("Category already exists.");

        var category = _mapper.Map<Category>(dto);

        category.IsActive = true;
        category.CreatedOn = DateTime.UtcNow;
        category.CreatedBy = _currentUserService.UserName ?? "System";

        await _unitOfWork.CategoryRepository.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<bool> UpdateAsync(int id, UpdateCategoryDto dto)
    {
        var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

        if (category == null)
            return false;

        _mapper.Map(dto, category);

        category.ModifiedOn = DateTime.UtcNow;
        category.ModifiedBy = _currentUserService.UserName ?? "System";

        _unitOfWork.CategoryRepository.Update(category);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

        if (category == null)
            return false;
        category.IsDeleted = true;
        category.DeletedOn = DateTime.UtcNow;
        category.DeletedBy = _currentUserService.UserName ?? "System";

        _unitOfWork.CategoryRepository.Update(category);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}