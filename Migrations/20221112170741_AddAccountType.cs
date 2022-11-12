using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace meta_menu_be.Migrations
{
    public partial class AddAccountType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersItems_FoodItems_ItemId",
                table: "OrdersItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdersItems_Orders_OrderId",
                table: "OrdersItems");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "OrdersItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "OrdersItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AccountType",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersItems_FoodItems_ItemId",
                table: "OrdersItems",
                column: "ItemId",
                principalTable: "FoodItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersItems_Orders_OrderId",
                table: "OrdersItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersItems_FoodItems_ItemId",
                table: "OrdersItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrdersItems_Orders_OrderId",
                table: "OrdersItems");

            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "OrdersItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "OrdersItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersItems_FoodItems_ItemId",
                table: "OrdersItems",
                column: "ItemId",
                principalTable: "FoodItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersItems_Orders_OrderId",
                table: "OrdersItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
