using GroceryShop.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

            builder.HasMany(_ => _.Imports)
                .WithOne(_ => _.Product)
                .HasForeignKey(_ => _.ProductCode)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasMany(_ => _.Sells)
            .WithOne(_ => _.Product)
            .HasForeignKey(_ => _.ProductCode)
            .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
