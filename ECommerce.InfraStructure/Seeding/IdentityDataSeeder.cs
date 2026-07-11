using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Identity;
using ECommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Seeding
{
    public class IdentityDataSeeder : IDataSeeder
    {
        private readonly StoreIdentityDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<IdentityDataSeeder> logger;

        public IdentityDataSeeder(StoreIdentityDbContext context, UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager, ILogger<IdentityDataSeeder> logger)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }
        public async Task SeedAsync(CancellationToken ct = default)
        {
            try
            {
                var pending = await context.Database.GetPendingMigrationsAsync(ct);
                if (pending.Count() > 0)
                    await context.Database.MigrateAsync(ct);

                if (!await roleManager.Roles.AnyAsync(ct))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                    await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!await userManager.Users.AnyAsync(ct))
                {
                    var admin = new ApplicationUser()
                    {
                        DisplayName = "Mohamed Ahmed",
                        Email = "Mohamed@gmail.com",
                        UserName = "Mohamed",
                        PhoneNumber = "01005249114"
                    };
                    var result = await userManager.CreateAsync(admin, "P@ssw0rd");
                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(admin, "Admin");
                    else
                        logger.LogWarning("User did not create");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Can not Seed the Data");
                throw;
            }
        }
    }
}