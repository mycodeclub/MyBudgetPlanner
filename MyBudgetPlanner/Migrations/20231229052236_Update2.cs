using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBudgetPlanner.Migrations
{
    /// <inheritdoc />
    public partial class Update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyExpenses_ApplicationUser_UserId",
                table: "MyExpenses");

            migrationBuilder.DropTable(
                name: "ExpenseTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyExpenses",
                table: "MyExpenses");

            migrationBuilder.RenameTable(
                name: "MyExpenses",
                newName: "MyDailyExpenses");

            migrationBuilder.RenameColumn(
                name: "ExpenseTags",
                table: "MyExpensePlans",
                newName: "ExpenseTagCommaList");

            migrationBuilder.RenameColumn(
                name: "ExpenseTags",
                table: "MyDailyExpenses",
                newName: "ExpenseTagCommaList");

            migrationBuilder.RenameIndex(
                name: "IX_MyExpenses_UserId",
                table: "MyDailyExpenses",
                newName: "IX_MyDailyExpenses_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyDailyExpenses",
                table: "MyDailyExpenses",
                column: "UniqueId");

            migrationBuilder.CreateTable(
                name: "ExpenseTagMaster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseTagMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseTagMaster_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTagMaster_UserId",
                table: "ExpenseTagMaster",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MyDailyExpenses_ApplicationUser_UserId",
                table: "MyDailyExpenses",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyDailyExpenses_ApplicationUser_UserId",
                table: "MyDailyExpenses");

            migrationBuilder.DropTable(
                name: "ExpenseTagMaster");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MyDailyExpenses",
                table: "MyDailyExpenses");

            migrationBuilder.RenameTable(
                name: "MyDailyExpenses",
                newName: "MyExpenses");

            migrationBuilder.RenameColumn(
                name: "ExpenseTagCommaList",
                table: "MyExpensePlans",
                newName: "ExpenseTags");

            migrationBuilder.RenameColumn(
                name: "ExpenseTagCommaList",
                table: "MyExpenses",
                newName: "ExpenseTags");

            migrationBuilder.RenameIndex(
                name: "IX_MyDailyExpenses_UserId",
                table: "MyExpenses",
                newName: "IX_MyExpenses_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MyExpenses",
                table: "MyExpenses",
                column: "UniqueId");

            migrationBuilder.CreateTable(
                name: "ExpenseTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    TagName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseTags_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTags_UserId",
                table: "ExpenseTags",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MyExpenses_ApplicationUser_UserId",
                table: "MyExpenses",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");
        }
    }
}
