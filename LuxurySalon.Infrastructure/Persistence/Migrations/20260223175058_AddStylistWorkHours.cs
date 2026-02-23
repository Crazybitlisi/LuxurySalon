using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxurySalon.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStylistWorkHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkEndTime",
                table: "Stylists",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkStartTime",
                table: "Stylists",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkEndTime",
                table: "Stylists");

            migrationBuilder.DropColumn(
                name: "WorkStartTime",
                table: "Stylists");
        }
    }
}
