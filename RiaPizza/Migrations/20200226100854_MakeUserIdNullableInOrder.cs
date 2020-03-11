using Microsoft.EntityFrameworkCore.Migrations;

namespace RiaPizza.Migrations
{
    public partial class MakeUserIdNullableInOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderDeliveryAddresses_OrderId",
                table: "OrderDeliveryAddresses");

            migrationBuilder.DropIndex(
                name: "IX_OrderBy_OrderId",
                table: "OrderBy");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDeliveryAddresses_OrderId",
                table: "OrderDeliveryAddresses",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderBy_OrderId",
                table: "OrderBy",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderDeliveryAddresses_OrderId",
                table: "OrderDeliveryAddresses");

            migrationBuilder.DropIndex(
                name: "IX_OrderBy_OrderId",
                table: "OrderBy");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDeliveryAddresses_OrderId",
                table: "OrderDeliveryAddresses",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderBy_OrderId",
                table: "OrderBy",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_UserId",
                table: "Orders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
