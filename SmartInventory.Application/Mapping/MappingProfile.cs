using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SmartInventory.Application.DTOs.Audit;
using SmartInventory.Application.DTOs.Category;
using SmartInventory.Application.DTOs.Customer;
using SmartInventory.Application.DTOs.Invoice;
using SmartInventory.Application.DTOs.Notification;
using SmartInventory.Application.DTOs.Product;
using SmartInventory.Application.DTOs.Purchase;
using SmartInventory.Application.DTOs.Reports;
using SmartInventory.Application.DTOs.Role;
using SmartInventory.Application.DTOs.Sale;
using SmartInventory.Application.DTOs.Settings;
using SmartInventory.Application.DTOs.Supplier;
using SmartInventory.Application.DTOs.User;
using SmartInventory.Domain.Entities;

namespace SmartInventory.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AppSetting, AppSettingDto>();
        CreateMap<UpdateAppSettingDto, AppSetting>();

        CreateMap<ApplicationUser, UserDto>();
        CreateMap<UpdateUserDto, ApplicationUser>();

        //CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Product, ProductDto>().ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category != null ? s.Category.Name : string.Empty));
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
        CreateMap<Product, StockReportDto>();

        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();

        CreateMap<Supplier, SupplierDto>().ReverseMap();
        CreateMap<CreateSupplierDto, Supplier>();
        CreateMap<UpdateSupplierDto, Supplier>();

        CreateMap<Purchase, PurchaseDto>().ForMember(dest => dest.SupplierName,opt => opt.MapFrom(src => src.Supplier.Name)).ForMember(dest => dest.Items,opt => opt.MapFrom(src => src.PurchaseItems));
        CreateMap<PurchaseItem, PurchaseItemDto>().ForMember(dest => dest.ProductName,opt => opt.MapFrom(src => src.Product.Name));
        CreateMap<CreatePurchaseDto, Purchase>();
        CreateMap<Purchase, PurchaseReportDto>();
        CreateMap<CreatePurchaseItemDto, PurchaseItem>();

        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>();

        CreateMap<Sale, SaleDto>().ForMember(dest => dest.CustomerName,opt => opt.MapFrom(src => src.Customer.Name)).ForMember(dest => dest.Items,opt => opt.MapFrom(src => src.SaleItems));
        CreateMap<SaleItem, SaleItemDto>().ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
        CreateMap<CreateSaleDto, Sale>();
        CreateMap<CreateSaleItemDto, SaleItem>();

        CreateMap<Sale, InvoiceDto>().ForMember(d => d.InvoiceDate,o => o.MapFrom(s => s.SaleDate)).ForMember(d => d.CustomerName,o => o.MapFrom(s => s.Customer.Name))
            .ForMember(d => d.CustomerPhone, o => o.MapFrom(s => s.Customer.Phone)).ForMember(d => d.CustomerAddress,o => o.MapFrom(s => s.Customer.Address)).ForMember(d => d.Items,o => o.MapFrom(s => s.SaleItems));
        CreateMap<SaleItem, InvoiceItemDto>().ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name));

        CreateMap<SalesReportDto, Sale>().ReverseMap();
        CreateMap<SaleItem, SalesReportItemDto>().ReverseMap();

        CreateMap<AuditLog, AuditLogDto>();
        CreateMap<IdentityRole, RoleDto>();

        CreateMap<Notification, NotificationDto>();
    }
}