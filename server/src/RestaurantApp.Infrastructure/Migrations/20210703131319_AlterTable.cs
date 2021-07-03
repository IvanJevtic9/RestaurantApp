using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantApp.Infrastructure.Migrations
{
    public partial class AlterTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ItemPrice",
                table: "MenuItems",
                newName: "Attributes");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "MenuItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "MenuItems");

            migrationBuilder.RenameColumn(
                name: "Attributes",
                table: "MenuItems",
                newName: "ItemPrice");
        }
    }
}
