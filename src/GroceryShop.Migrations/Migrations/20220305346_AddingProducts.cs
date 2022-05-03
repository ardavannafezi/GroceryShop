using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Migrations
{
    [Migration(20220305346)]
    public class _20220305346 : Migration
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
                            .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
                            .WithColumn("Name").AsString(50).NotNullable()
                            .WithColumn("CategoryId").AsInt32().NotNullable()
                            .ForeignKey("FK_Products_Categories", "Categories", "Id")
                            .WithColumn("BuyPrice").AsDouble().NotNullable()
                            .WithColumn("SellPrice").AsDouble().NotNullable()
                            .WithColumn("Quantity").AsInt32().NotNullable()
                            .WithColumn("MaxInStock").AsInt32()
                            .WithColumn("MinInStock").AsInt32();


        }

    }
}
