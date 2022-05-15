using GroceryShop.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GroceryShop.Persistence.EF.Categories
{
    public class CategoryEntityMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(_ => _.Id);

            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd();

            builder.Property(_ => _.Name);

            builder.HasMany(_ => _.Products)
                .WithOne(_ => _.Category)
                .HasForeignKey(_ => _.CategoryId)
                .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
