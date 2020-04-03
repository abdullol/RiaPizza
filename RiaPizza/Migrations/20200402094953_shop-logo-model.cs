using Microsoft.EntityFrameworkCore.Migrations;

namespace RiaPizza.Migrations
{
    public partial class shoplogomodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShopLogo",
                table: "ShopSchedule",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShopLogo",
                table: "ShopSchedule");

            migrationBuilder.AddColumn<int>(
                name: "OrderInstance",
                table: "DeliveryAreas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAreas_OrderInstance",
                table: "DeliveryAreas",
                column: "OrderInstance",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryAreas_Orders_OrderInstance",
                table: "DeliveryAreas",
                column: "OrderInstance",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
