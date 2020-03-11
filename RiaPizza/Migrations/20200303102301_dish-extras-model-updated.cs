using Microsoft.EntityFrameworkCore.Migrations;

namespace RiaPizza.Migrations
{
    public partial class dishextrasmodelupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Allergies",
                table: "DishExtras",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Allergies",
                table: "Dishes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Allergies",
                table: "DishExtras");

            migrationBuilder.DropColumn(
                name: "Allergies",
                table: "Dishes");
        }
    }
}
