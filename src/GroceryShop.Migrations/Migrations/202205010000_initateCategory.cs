using FluentMigrator;

namespace GroceryShop.Migrations
{
    [Migration(202205010000)]
    public class _202205010000 : Migration
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
                            .WithColumn("Name").AsString(50).NotNullable();
        }

    }
}
