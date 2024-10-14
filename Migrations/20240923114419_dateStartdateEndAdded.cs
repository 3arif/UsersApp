using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UsersApp.Migrations
{
    /// <inheritdoc />
    public partial class dateStartdateEndAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "time",
                table: "Attendance",
                newName: "timeStart");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "timeEnd",
                table: "Attendance",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "timeEnd",
                table: "Attendance");

            migrationBuilder.RenameColumn(
                name: "timeStart",
                table: "Attendance",
                newName: "time");
        }
    }
}
