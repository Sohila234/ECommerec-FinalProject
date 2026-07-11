using ECommerce.Domain.Contracts;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Extentaions
{
    public static class ProgramExtentions
    {
        public static async Task MigrationAndSeedAsync (this WebApplication app )
        {
            var scope =  app.Services.CreateScope ();
            var Seeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Catalog");
            await Seeder.SeedAsync();
            var IdentitySeeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Identity");
            await IdentitySeeder.SeedAsync();
        }
    }
}
