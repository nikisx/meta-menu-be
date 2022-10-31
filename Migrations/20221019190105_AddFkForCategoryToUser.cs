using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace meta_menu_be.Migrations
{
    public partial class AddFkForCategoryToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FoodCategories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_FoodCategories_UserId",
                table: "FoodCategories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodCategories_AspNetUsers_UserId",
                table: "FoodCategories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodCategories_AspNetUsers_UserId",
                table: "FoodCategories");

            migrationBuilder.DropIndex(
                name: "IX_FoodCategories_UserId",
                table: "FoodCategories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FoodCategories");
        }
    }
}
