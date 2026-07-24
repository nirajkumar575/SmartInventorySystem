using AutoMapper;
using SmartInventory.Application.DTOs.Product;
using SmartInventory.Application.Interfaces;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;

namespace SmartInventory.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    //private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    //public async Task<IEnumerable<ProductDto>> GetAllAsync()
    //{
    //    var products = await _productRepository.GetAllAsync();
    //    return _mapper.Map<IEnumerable<ProductDto>>(products);
    //}
    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();

        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            SKU = p.SKU,
            Price = p.Price,
            Quantity = p.Quantity
        });
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
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
            return null;

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            SKU = product.SKU,
            Price = product.Price,
            Quantity = product.Quantity
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
        if (await _productRepository.ExistsAsync(x => x.SKU == dto.SKU))
            throw new Exception("Product SKU already exists.");

        var product = new Product
        {
            Name = dto.Name,
            SKU = dto.SKU,
            Price = dto.Price,
            Quantity = dto.Quantity
        };

        await _productRepository.AddAsync(product);
        await _productRepository.SaveChangesAsync();

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
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
            return false;

        product.Name = dto.Name;
        product.Price = dto.Price;
        product.Quantity = dto.Quantity;
        product.ModifiedOn = DateTime.UtcNow;

        _productRepository.Update(product);
        await _productRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
            return false;

        _productRepository.Delete(product);
        await _productRepository.SaveChangesAsync();

        return true;
    }
}