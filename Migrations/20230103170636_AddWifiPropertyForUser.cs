using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace meta_menu_be.Migrations
{
    public partial class AddWifiPropertyForUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Wifi",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Wifi",
                table: "AspNetUsers");
        }
    }
}
