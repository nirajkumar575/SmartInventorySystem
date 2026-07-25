using AutoMapper;
using Microsoft.Extensions.Logging;
using SmartInventory.Application.DTOs.Product;
using SmartInventory.Application.Exceptions;
using SmartInventory.Application.Interfaces;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;
using SmartInventory.Shared.Common;
using SmartInventory.Shared.QueryParameters;

namespace SmartInventory.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProductService> _logger;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper,ILogger<ProductService> logger,ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _currentUserService = currentUserService;
    }
    public async Task<PagedResult<ProductDto>> GetAllAsync(ProductQueryParameters request)
    {
        var result = await _unitOfWork.ProductRepository.GetPagedProductsAsync(request);

        return new PagedResult<ProductDto>
        {
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords,
            Items = _mapper.Map<IEnumerable<ProductDto>>(result.Items)
        };
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

        if (product == null)
            return null;

        return _mapper.Map<ProductDto>(product);
    }
    
    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        if (await _unitOfWork.ProductRepository.ExistsAsync(x => x.SKU == dto.SKU))
            throw new BadRequestException("Product SKU already exists.");

        var product = _mapper.Map<Product>(dto);
        Console.WriteLine($"CategoryId: {product.CategoryId}");
        product.CreatedOn = DateTime.UtcNow;
        product.CreatedBy = _currentUserService.UserName ?? "System";

        _logger.LogInformation("Creating new product with SKU {SKU}",dto.SKU);
        await _unitOfWork.ProductRepository.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);
    }
    public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

        if (product == null)
            return false;

        _mapper.Map(dto, product);

        product.ModifiedOn = DateTime.UtcNow;
        product.ModifiedBy = _currentUserService.UserName ?? "System";

        _logger.LogInformation("Updating Product {Id}", id);
        _unitOfWork.ProductRepository.Update(product);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

        if (product == null)
            return false;

        _logger.LogWarning("Deleting product Id {Id}",id);

        product.IsDeleted = true;
        product.DeletedOn = DateTime.UtcNow;
        product.DeletedBy = _currentUserService.UserName ?? "System";

        _unitOfWork.ProductRepository.Update(product);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}