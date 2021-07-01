using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantApp.Infrastructure.Migrations
{
    public partial class RenameImageColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImangeName",
                table: "Images",
                newName: "ImageName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Images",
                newName: "ImangeName");
        }
    }
}
