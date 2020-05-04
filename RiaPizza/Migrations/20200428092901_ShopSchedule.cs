using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RiaPizza.Migrations
{
    public partial class ShopSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShopSchedule",
                table: "ShopSchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryTimings",
                table: "DeliveryTimings");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "ShopSchedule");

            migrationBuilder.DropColumn(
                name: "TimeFrom",
                table: "ShopSchedule");

            migrationBuilder.DropColumn(
                name: "TimeTo",
                table: "ShopSchedule");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DeliveryTimings");

            migrationBuilder.DropColumn(
                name: "Timings",
                table: "DeliveryTimings");

            migrationBuilder.AddColumn<int>(
                name: "ShopScheduleId",
                table: "ShopSchedule",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "DeliveryTimingId",
                table: "DeliveryTimings",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "DeliveryTimings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShopScheduleId",
                table: "DeliveryTimings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeFrom",
                table: "DeliveryTimings",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeTo",
                table: "DeliveryTimings",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShopSchedule",
                table: "ShopSchedule",
                column: "ShopScheduleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryTimings",
                table: "DeliveryTimings",
                column: "DeliveryTimingId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryTimings_ShopScheduleId",
                table: "DeliveryTimings",
                column: "ShopScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryTimings_ShopSchedule_ShopScheduleId",
                table: "DeliveryTimings",
                column: "ShopScheduleId",
                principalTable: "ShopSchedule",
                principalColumn: "ShopScheduleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryTimings_ShopSchedule_ShopScheduleId",
                table: "DeliveryTimings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShopSchedule",
                table: "ShopSchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryTimings",
                table: "DeliveryTimings");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryTimings_ShopScheduleId",
                table: "DeliveryTimings");

            migrationBuilder.DropColumn(
                name: "ShopScheduleId",
                table: "ShopSchedule");

            migrationBuilder.DropColumn(
                name: "DeliveryTimingId",
                table: "DeliveryTimings");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "DeliveryTimings");

            migrationBuilder.DropColumn(
                name: "ShopScheduleId",
                table: "DeliveryTimings");

            migrationBuilder.DropColumn(
                name: "TimeFrom",
                table: "DeliveryTimings");

            migrationBuilder.DropColumn(
                name: "TimeTo",
                table: "DeliveryTimings");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "ShopSchedule",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeFrom",
                table: "ShopSchedule",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeTo",
                table: "ShopSchedule",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DeliveryTimings",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Timings",
                table: "DeliveryTimings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShopSchedule",
                table: "ShopSchedule",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryTimings",
                table: "DeliveryTimings",
                column: "Id");
        }
    }
}
