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
    //private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork,ILogger<ProductService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    //public async Task<IEnumerable<ProductDto>> GetAllAsync()
    //{
    //    var products = await _productRepository.GetAllAsync();
    //    return _mapper.Map<IEnumerable<ProductDto>>(products);
    //}

    public async Task<PagedResult<ProductDto>> GetAllAsync(ProductQueryParameters request)
    {
        var result = await _unitOfWork.ProductRepository.GetPagedProductsAsync(request);

        return new PagedResult<ProductDto>
        {
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalRecords = result.TotalRecords,
            Items = result.Items.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                SKU = p.SKU,
                Price = p.Price,
                Quantity = p.Quantity,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? string.Empty
            })
        };
    }

    //public async Task<ProductDto?> GetByIdAsync(int id)
    //{
    //    var product = await _productRepository.GetByIdAsync(id);

    //    if (product == null)
    //        return null;

    //    return _mapper.Map<ProductDto>(product);
    //}
    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

        if (product == null)
            return null;

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            SKU = product.SKU,
            Price = product.Price,
            Quantity = product.Quantity,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty
        };
    }
    //public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    //{
    //    if (await _productRepository.ExistsAsync(x => x.SKU == dto.SKU))
    //        throw new Exception("Product SKU already exists.");

    //    var product = _mapper.Map<Product>(dto);

    //    await _productRepository.AddAsync(product);
    //    await _productRepository.SaveChangesAsync();

    //    return _mapper.Map<ProductDto>(product);
    //}
    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        if (await _unitOfWork.ProductRepository.ExistsAsync(x => x.SKU == dto.SKU))
            throw new BadRequestException("Product SKU already exists.");

        var product = new Product
        {
            Name = dto.Name,
            SKU = dto.SKU,
            Price = dto.Price,
            Quantity = dto.Quantity,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "System"
        };
        _logger.LogInformation("Creating new product with SKU {SKU}",dto.SKU);
        await _unitOfWork.ProductRepository.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            SKU = product.SKU,
            Price = product.Price,
            Quantity = product.Quantity
        };
    }
    public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

        if (product == null)
            throw new NotFoundException("Product not found.");

        product.Name = dto.Name;
        product.Price = dto.Price;
        product.Quantity = dto.Quantity;
        product.ModifiedOn = DateTime.UtcNow;
        product.ModifiedBy = "System";

        _unitOfWork.ProductRepository.Update(product);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

        if (product == null)
            throw new NotFoundException("Product not found.");

        _logger.LogWarning("Deleting product Id {Id}",id);
        _unitOfWork.ProductRepository.Delete(product);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}