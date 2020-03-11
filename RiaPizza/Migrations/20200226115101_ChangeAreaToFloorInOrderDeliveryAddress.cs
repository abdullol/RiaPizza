using Microsoft.EntityFrameworkCore.Migrations;

namespace RiaPizza.Migrations
{
    public partial class ChangeAreaToFloorInOrderDeliveryAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "OrderDeliveryAddresses");

            migrationBuilder.AddColumn<string>(
                name: "Floor",
                table: "OrderDeliveryAddresses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Floor",
                table: "OrderDeliveryAddresses");

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "OrderDeliveryAddresses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
