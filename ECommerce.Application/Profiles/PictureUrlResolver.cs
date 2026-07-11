using AutoMapper;
using AutoMapper.Execution;
using ECommerce.Application.DTO_S.Products;
using ECommerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using static ECommerce.Application.Profiles.PictureUrlResolver;

namespace ECommerce.Application.Profiles
{
    public class PictureUrlResolver(IOptions<UrlSettings> options) : IValueResolver<Product, ProductDto, string?>
    {
        private readonly UrlSettings _urlSettings = options.Value;
        public string? Resolve(Product source, ProductDto destination, string? destMember, ResolutionContext context)
        {
            if(string.IsNullOrEmpty(source.PictureUrl)) return null;
            var BaseUrl =  _urlSettings.BaseUrl.TrimEnd("/");
            var Path=source.PictureUrl.TrimStart("/");
            return $"{BaseUrl}/Files/{Path}";

        }
        public class UrlSettings
        {
            public string BaseUrl { get; set; } = string.Empty;

        }
    }
}
