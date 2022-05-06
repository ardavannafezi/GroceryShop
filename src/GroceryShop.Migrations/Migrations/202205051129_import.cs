using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Migrations
{
    [Migration(202205051129)]
    public class _202205051129 : Migration
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
                            .WithColumn("Price").AsDouble().NotNullable()
                            .WithColumn("Quantity").AsInt32().NotNullable();



        }

    }
}
