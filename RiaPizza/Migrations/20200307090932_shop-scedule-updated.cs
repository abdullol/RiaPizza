using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RiaPizza.Migrations
{
    public partial class shopsceduleupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "openFrom",
                table: "ShopSchedule");

            migrationBuilder.DropColumn(
                name: "openTo",
                table: "ShopSchedule");

            migrationBuilder.RenameColumn(
                name: "isOpen",
                table: "ShopSchedule",
                newName: "IsOpen");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeFrom",
                table: "ShopSchedule",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeTo",
                table: "ShopSchedule",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeFrom",
                table: "ShopSchedule");

            migrationBuilder.DropColumn(
                name: "TimeTo",
                table: "ShopSchedule");

            migrationBuilder.RenameColumn(
                name: "IsOpen",
                table: "ShopSchedule",
                newName: "isOpen");

            migrationBuilder.AddColumn<string>(
                name: "openFrom",
                table: "ShopSchedule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "openTo",
                table: "ShopSchedule",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
