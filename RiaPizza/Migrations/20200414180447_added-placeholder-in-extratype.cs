using Microsoft.EntityFrameworkCore.Migrations;

namespace RiaPizza.Migrations
{
    public partial class addedplaceholderinextratype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Placeholder",
                table: "DishExtraTypes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Placeholder",
                table: "DishExtraTypes");
        }
    }
}
