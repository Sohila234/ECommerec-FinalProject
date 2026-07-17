using ECommerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Data.Configuration
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(x => x.Cost).HasColumnType("decimal(10,2)");
            builder.Property(x => x.ShortName).HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(x => x.Description).HasColumnType("varchar(100)").HasMaxLength(100);
            builder.Property(x => x.DeliveryTime).HasColumnType("varchar(50)").HasMaxLength(50);



        }
    }
}
