using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.Products;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ECommerce.Infrastructure.Seeding
{
    public class CatalogDataSeeder(StoreDBContext dBContext, ILogger<CatalogDataSeeder> logger) : IDataSeeder 
    {
        public async Task SeedAsync(CancellationToken ct = default)
        {
            try
            {
                var Pending = await dBContext.Database.GetPendingMigrationsAsync();
                if (Pending.Count() > 0)
                {
                    await dBContext.Database.MigrateAsync();
                }
                var SeedPath = Path.Combine(AppContext.BaseDirectory, "DataSeed");
                await SeedIfEmptyAsync<ProductsBrand>(SeedPath, "brands.json",ct);
                await SeedIfEmptyAsync<ProductsType>(SeedPath, "types.json", ct);
                await SeedIfEmptyAsync<Product>(SeedPath, "products.json", ct);
                await SeedIfEmptyAsync<DeliveryMethod>(SeedPath, "delivery.json", ct);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed To Seed Data");
                throw;
            }

        }
        private async Task SeedIfEmptyAsync<T>(string Root, string FileName, CancellationToken ct = default) where T : class
        {
            if (await dBContext.Set<T>().AnyAsync(ct)) return;
            var FilePath = Path.Combine(Root, FileName);
            if (!File.Exists(FilePath))
            {
                logger.LogWarning($"Seed File Not Found : {FileName}");
                return;
            }

            try
            {
                await using var stream = File.OpenRead(FilePath);

                // تعديل الخيارات لتفادي مشاكل العلاقات والـ Reference Loop
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
                };

                var Item = await JsonSerializer.DeserializeAsync<List<T>>(stream, options, ct);

                if (Item != null && Item.Count > 0)
                {
                    await dBContext.Set<T>().AddRangeAsync(Item, ct);
                    await dBContext.SaveChangesAsync(ct);
                    logger.LogInformation($"Successfully Seeded {FileName} with {Item.Count} items.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error while deserializing or saving seed data for file: {FileName}");
            }
        }
    }
}
