using FluentMigrator;

namespace GroceryShop.Migrations
{
    [Migration(202205020000)]
    public class _202205020000 : Migration
    {

        public override void Up()
        {
            CreateProduct();
            
        }

        public override void Down()
        {
            Delete.Table("Products");
        }

        private void CreateProduct()
        {
            Create.Table("Products")
                            .WithColumn("ProductCode").AsInt32().PrimaryKey().NotNullable().Unique()
                            .WithColumn("Name").AsString(50).NotNullable()
                            .WithColumn("CategoryId").AsInt32().NotNullable()
                            .ForeignKey("FK_Products_Categories", "Categories", "Id")
                            .WithColumn("Quantity").AsInt32().NotNullable()
                            .WithColumn("MaxInStock").AsInt32()
                            .WithColumn("MinInStock").AsInt32();
        }
    }
}
