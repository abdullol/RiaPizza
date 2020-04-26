using Microsoft.EntityFrameworkCore.Migrations;

namespace RiaPizza.Migrations
{
    public partial class addeddeliverychargesinorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "DeliveryCharges",
                table: "Orders",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryCharges",
                table: "Orders");
        }
    }
}
