using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RiaPizza.Migrations
{
    public partial class removetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryTimings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryTimings",
                columns: table => new
                {
                    DeliveryTimingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    ShopScheduleId = table.Column<int>(type: "int", nullable: false),
                    TimeFrom = table.Column<TimeSpan>(type: "time", nullable: false),
                    TimeTo = table.Column<TimeSpan>(type: "time", nullable: false)
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
    }
}
