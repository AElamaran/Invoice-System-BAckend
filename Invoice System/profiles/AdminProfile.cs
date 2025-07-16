using AutoMapper;
using Invoice_System.DTOs.Admin;
using Invoice_System.Models;

namespace Invoice_System.profiles
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            // User Mapping
            CreateMap<UserDto, User>();
            CreateMap<UpdateUserDto, User>();

            // Product Mapping
            CreateMap<ProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();

            // Invoice Mapping
            CreateMap<CreateInvoiceDto, Invoice>();
            CreateMap<InvoiceDto, Invoice>();

            // Invoice Item Mapping
            CreateMap<InvoiceItemDto, InvoiceItem>();
        }

    }
}
