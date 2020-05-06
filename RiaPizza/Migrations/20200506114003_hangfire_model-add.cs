using Microsoft.EntityFrameworkCore.Migrations;

namespace RiaPizza.Migrations
{
    public partial class hangfire_modeladd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShopSchedule",
                columns: table => new
                {
                    ShopScheduleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsOpen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopSchedule", x => x.ShopScheduleId);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryTimings",
                columns: table => new
                {
                    DeliveryTimingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<string>(nullable: true),
                    DayOfWeek = table.Column<int>(nullable: false),
                    TimeFrom = table.Column<int>(nullable: false),
                    TimeTo = table.Column<int>(nullable: false),
                    ShopScheduleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryTimings", x => x.DeliveryTimingId);
                    table.ForeignKey(
                        name: "FK_DeliveryTimings_ShopSchedule_ShopScheduleId",
                        column: x => x.ShopScheduleId,
                        principalTable: "ShopSchedule",
                        principalColumn: "ShopScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTimings_ShopScheduleId",
                table: "DeliveryTimings",
                column: "ShopScheduleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryTimings");

            migrationBuilder.DropTable(
                name: "ShopSchedule");
        }
    }
}
