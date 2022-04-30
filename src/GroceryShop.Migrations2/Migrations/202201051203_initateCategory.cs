using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Migrations
{
    [Migration(202201051203)]
    public class _202201051203_initiateCategory : Migration
    {

        public override void Up()
        {
            CreateCategory();
            
        }

        public override void Down()
        {
            Delete.Table("Categories");
        }

        private void CreateCategory()
        {
            Create.Table("Categories")
                            .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
                            .WithColumn("Title").AsString(50).NotNullable();
        }

    }
}
