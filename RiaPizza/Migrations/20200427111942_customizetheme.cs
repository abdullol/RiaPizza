using Microsoft.EntityFrameworkCore.Migrations;

namespace RiaPizza.Migrations
{
    public partial class customizetheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShopLogo",
                table: "ShopSchedule");

            migrationBuilder.CreateTable(
                name: "CustomizeTheme",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Logo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomizeTheme", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomizeTheme");

            migrationBuilder.AddColumn<string>(
                name: "ShopLogo",
                table: "ShopSchedule",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
