using GroceryShop.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroceryShop.Persistence.EF.Sells
{
    public class SellsEntityMap : IEntityTypeConfiguration<Sell>
    {
        public void Configure(EntityTypeBuilder<Sell> builder)
        {
            builder.ToTable("Sells");

            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd();

            builder.Property(_ => _.Quantity);

            builder.Property(_ => _.dateTime);

            builder.Property(_ => _.ProductCode);

        }
    }
}
