using Microsoft.EntityFrameworkCore.Migrations;

namespace RiaPizza.Migrations
{
    public partial class Addedtipinorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Tip",
                table: "Orders",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tip",
                table: "Orders");
        }
    }
}
