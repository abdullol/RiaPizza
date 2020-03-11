using Microsoft.EntityFrameworkCore.Migrations;

namespace RiaPizza.Migrations
{
    public partial class ProjectMerging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "DishCategories",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryCharges",
                table: "DeliveryAreas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinOrderCharges",
                table: "DeliveryAreas",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "DishCategories");

            migrationBuilder.DropColumn(
                name: "DeliveryCharges",
                table: "DeliveryAreas");

            migrationBuilder.DropColumn(
                name: "MinOrderCharges",
                table: "DeliveryAreas");
        }
    }
}
