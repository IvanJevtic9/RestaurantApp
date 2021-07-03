using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantApp.Infrastructure.Migrations
{
    public partial class RenameMenuItemColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TagPrice",
                table: "MenuItems",
                newName: "ItemPrice");

            migrationBuilder.RenameColumn(
                name: "DishDescription",
                table: "MenuItems",
                newName: "ItemDescription");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ItemPrice",
                table: "MenuItems",
                newName: "TagPrice");

            migrationBuilder.RenameColumn(
                name: "ItemDescription",
                table: "MenuItems",
                newName: "DishDescription");
        }
    }
}
