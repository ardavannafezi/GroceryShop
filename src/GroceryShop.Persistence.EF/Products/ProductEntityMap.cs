using GroceryShop.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Persistence.EF.Categories
{
    public class ProductEntityMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(_ => _.ProductCode);
            builder.Property(_ => _.ProductCode);
                
            builder.Property(_ => _.Name);

            builder.Property(_ => _.MaxInStock);

            builder.Property(_ => _.MinInStock);

            builder.Property(_ => _.Quantity);

            builder.Property(_ => _.SellPrice);

            builder.Property(_ => _.BuyPrice);



            builder.HasMany(_ => _.Imports)
                .WithOne(_ => _.Product)
                .HasForeignKey(_ => _.ProductCode)
                .OnDelete(DeleteBehavior.ClientNoAction);

        }
    }
}
