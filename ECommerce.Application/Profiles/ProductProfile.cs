using AutoMapper;
using ECommerce.Application.DTO_S.Products;
using ECommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Profiles
{
    public class ProductProfile :Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductsBrand, BrandDto>();
            CreateMap<ProductsType, TypeDto>();
            CreateMap<Product, ProductDto>()
                .ForMember(des => des.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(des => des.TypeName, opt => opt.MapFrom(src => src.Type.Name))
                .ForMember(des => des.PictureUrl, opt => opt.MapFrom < PictureUrlResolver>());
;



        }
    }
}
