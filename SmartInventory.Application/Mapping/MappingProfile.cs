using AutoMapper;
using SmartInventory.Application.DTOs.Category;
using SmartInventory.Application.DTOs.Customer;
using SmartInventory.Application.DTOs.Product;
using SmartInventory.Application.DTOs.Purchase;
using SmartInventory.Application.DTOs.Sale;
using SmartInventory.Application.DTOs.Supplier;
using SmartInventory.Domain.Entities;

namespace SmartInventory.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Product, ProductDto>().ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category != null ? s.Category.Name : string.Empty));
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();

        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();

        CreateMap<Supplier, SupplierDto>().ReverseMap();
        CreateMap<CreateSupplierDto, Supplier>();
        CreateMap<UpdateSupplierDto, Supplier>();

        CreateMap<Purchase, PurchaseDto>().ForMember(dest => dest.SupplierName,opt => opt.MapFrom(src => src.Supplier.Name)).ForMember(dest => dest.Items,opt => opt.MapFrom(src => src.PurchaseItems));
        CreateMap<PurchaseItem, PurchaseItemDto>().ForMember(dest => dest.ProductName,opt => opt.MapFrom(src => src.Product.Name));
        CreateMap<CreatePurchaseDto, Purchase>();
        CreateMap<CreatePurchaseItemDto, PurchaseItem>();

        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>();

        CreateMap<Sale, SaleDto>().ForMember(dest => dest.CustomerName,opt => opt.MapFrom(src => src.Customer.Name)).ForMember(dest => dest.Items,opt => opt.MapFrom(src => src.SaleItems));
        CreateMap<SaleItem, SaleItemDto>().ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
        CreateMap<CreateSaleDto, Sale>();
        CreateMap<CreateSaleItemDto, SaleItem>();
    }
}