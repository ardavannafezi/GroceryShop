using FluentMigrator;

namespace GroceryShop.Migrations
{
    [Migration(202205040000)]
    public class _202205040000 : Migration
    {

        public override void Up()
        {
            CreateSells();
            
        }

        public override void Down()
        {
            Delete.Table("Sells");
        }

        private void CreateSells()
        {
            Create.Table("Sells")
                            .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
                            .WithColumn("dateTime").AsDateTime2()
                            .WithColumn("ProductCode").AsInt32()
                            .ForeignKey("FK_Sells_Products", "Products", "ProductCode")
                            .WithColumn("Quantity").AsInt32().NotNullable();
                            



        }

    }
}
