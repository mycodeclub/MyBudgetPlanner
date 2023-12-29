using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBudgetPlanner.Migrations
{
    /// <inheritdoc />
    public partial class FixTypoDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Discription",
                table: "MyDailyExpenses",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "MyDailyExpenses",
                newName: "Discription");
        }
    }
}
