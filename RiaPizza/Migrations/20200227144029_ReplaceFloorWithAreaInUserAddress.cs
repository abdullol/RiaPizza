using Microsoft.EntityFrameworkCore.Migrations;

namespace RiaPizza.Migrations
{
    public partial class ReplaceFloorWithAreaInUserAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "AppUserAddresses");

            migrationBuilder.AddColumn<string>(
                name: "Floor",
                table: "AppUserAddresses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Floor",
                table: "AppUserAddresses");

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "AppUserAddresses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
