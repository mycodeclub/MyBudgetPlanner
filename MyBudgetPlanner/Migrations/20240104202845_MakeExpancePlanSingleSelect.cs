using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBudgetPlanner.Migrations
{
    /// <inheritdoc />
    public partial class MakeExpancePlanSingleSelect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpenseTagCommaList",
                table: "MyDailyExpenses");

            migrationBuilder.RenameColumn(
                name: "ExpenceDate",
                table: "MyDailyExpenses",
                newName: "ExpenseDate");

            migrationBuilder.AddColumn<Guid>(
                name: "ExpenseUnderPlanId",
                table: "MyDailyExpenses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MyDailyExpenses_ExpenseUnderPlanId",
                table: "MyDailyExpenses",
                column: "ExpenseUnderPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_MyDailyExpenses_MyExpensePlans_ExpenseUnderPlanId",
                table: "MyDailyExpenses",
                column: "ExpenseUnderPlanId",
                principalTable: "MyExpensePlans",
                principalColumn: "UniqueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyDailyExpenses_MyExpensePlans_ExpenseUnderPlanId",
                table: "MyDailyExpenses");

            migrationBuilder.DropIndex(
                name: "IX_MyDailyExpenses_ExpenseUnderPlanId",
                table: "MyDailyExpenses");

            migrationBuilder.DropColumn(
                name: "ExpenseUnderPlanId",
                table: "MyDailyExpenses");

            migrationBuilder.RenameColumn(
                name: "ExpenseDate",
                table: "MyDailyExpenses",
                newName: "ExpenceDate");

            migrationBuilder.AddColumn<string>(
                name: "ExpenseTagCommaList",
                table: "MyDailyExpenses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
