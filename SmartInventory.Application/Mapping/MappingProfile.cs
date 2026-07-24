using AutoMapper;
using SmartInventory.Application.DTOs.Product;
using SmartInventory.Domain.Entities;

namespace SmartInventory.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>();

        CreateMap<CreateProductDto, Product>();

        CreateMap<UpdateProductDto, Product>();
    }
}