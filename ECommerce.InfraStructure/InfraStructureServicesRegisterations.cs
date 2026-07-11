using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Repsitories;
using ECommerce.Infrastructure.Seeding;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace ECommerce.Infrastructure
{
    public static class InfraStructureServicesRegisterations
    {
        public static IServiceCollection AddInfraStructureServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<StoreDBContext>(Options =>
            {
                Options.UseSqlServer(configuration.GetConnectionString("StoreDBContext"));
                
            });
            services.AddDbContext<StoreIdentityDbContext>(Options =>
            {
                Options.UseSqlServer(configuration.GetConnectionString("IdentityContext"));

            });
            services.AddKeyedScoped<IDataSeeder,CatalogDataSeeder>("Catalog");
            services.AddKeyedScoped<IDataSeeder,IdentityDataSeeder>("Identity");
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IConnectionMultiplexer>(opt =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection"));
            });
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddSingleton<ICacheRepository, CacheRepository>();
            services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<StoreIdentityDbContext>();
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.RequireHttpsMetadata = true;
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidIssuer = configuration.GetSection("JWT")["Issuer"],
                    ValidAudience= configuration.GetSection("JWT")["Audience"],
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWT")["SecretKey"]))


                };
            });
            return services;
        }
         
    }
}
