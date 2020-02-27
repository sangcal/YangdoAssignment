using Microsoft.EntityFrameworkCore.Migrations;

namespace YangdoDAO.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalHours",
                schema: "dbo",
                table: "TimeSheets",
                newName: "WorkedHours");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkedHours",
                schema: "dbo",
                table: "TimeSheets",
                newName: "TotalHours");
        }
    }
}
