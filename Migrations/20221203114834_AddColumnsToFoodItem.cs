using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace meta_menu_be.Migrations
{
    public partial class AddColumnsToFoodItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Allergens",
                table: "FoodItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "FoodItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "FoodItems",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "FoodItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Allergens",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "FoodItems");
        }
    }
}
