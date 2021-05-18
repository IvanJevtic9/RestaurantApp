using Microsoft.EntityFrameworkCore.Migrations;

namespace RestaurantApp.Infrastructure.Migrations
{
    public partial class AddedImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Accounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImangeName = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                    ImageLocation = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ImageId",
                table: "Accounts",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_Id",
                table: "Images",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Images_ImageId",
                table: "Accounts",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Images_ImageId",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_ImageId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Accounts");
        }
    }
}
