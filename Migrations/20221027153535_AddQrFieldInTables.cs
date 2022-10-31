using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace meta_menu_be.Migrations
{
    public partial class AddQrFieldInTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QrCodeUrl",
                table: "Tables",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QrCodeUrl",
                table: "Tables");
        }
    }
}
