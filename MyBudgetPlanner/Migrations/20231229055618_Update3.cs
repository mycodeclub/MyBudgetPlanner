using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBudgetPlanner.Migrations
{
    /// <inheritdoc />
    public partial class Update3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpenseTagCommaList",
                table: "MyExpensePlans",
                newName: "ExpenseName");

            migrationBuilder.RenameColumn(
                name: "Discription",
                table: "MyExpensePlans",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpenseName",
                table: "MyExpensePlans",
                newName: "ExpenseTagCommaList");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "MyExpensePlans",
                newName: "Discription");
        }
    }
}
