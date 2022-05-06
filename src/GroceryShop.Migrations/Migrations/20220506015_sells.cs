using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Migrations
{
    [Migration(20220506015)]
    public class _20220506015 : Migration
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
                            .WithColumn("ProductCode").AsInt32()
                            .ForeignKey("FK_Sells_Products", "Products", "ProductCode")
                            .WithColumn("Quantity").AsInt32().NotNullable();



        }

    }
}
