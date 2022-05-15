using GroceryShop.Entities;
using GroceryShop.Persistence.EF.Categories;
using Microsoft.EntityFrameworkCore;

namespace GroceryShop.Persistence.EF
{
    public class EFDataContext : DbContext
    {

        public EFDataContext(string connectionString) :
            this(new DbContextOptionsBuilder().UseSqlServer(connectionString).Options)
        { }

        public EFDataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly
                (typeof(CategoryEntityMap).Assembly);

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Import> Imports { get; set; }
        public DbSet<Sell> Sells { get; set; }


    }
}
