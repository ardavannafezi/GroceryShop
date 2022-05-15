using FluentMigrator;

namespace GroceryShop.Migrations
{
    [Migration(202205030000)]
    public class _202205030000 : Migration
    {

        public override void Up()
        {
            CreateImports();
            
        }

        public override void Down()
        {
            Delete.Table("Imports");
        }

        private void CreateImports()
        {
            Create.Table("Imports")
                            .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
                            .WithColumn("ProductCode").AsInt32()
                            .ForeignKey("FK_Imports_Products", "Products", "ProductCode")
                            .WithColumn("Quantity").AsInt32().NotNullable();



        }

    }
}
