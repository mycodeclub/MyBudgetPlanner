using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBudgetPlanner.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedExpDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dateday",
                table: "MyExpenses");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "MyExpenses");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "MyExpenses");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpenceDate",
                table: "MyExpenses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpenceDate",
                table: "MyExpenses");

            migrationBuilder.AddColumn<int>(
                name: "Dateday",
                table: "MyExpenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte>(
                name: "Month",
                table: "MyExpenses",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "MyExpenses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
