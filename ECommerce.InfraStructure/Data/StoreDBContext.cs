using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data
{
    public class StoreDBContext :DbContext
    {
        public StoreDBContext(DbContextOptions<StoreDBContext> Options):base(Options) 
        {
            
        }
        public  DbSet<Product> products { get; set; }
        public DbSet<ProductsBrand> productsBrands { get; set; }
        public DbSet<ProductsType> productsTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreDBContext).Assembly);

        }


    }
}
