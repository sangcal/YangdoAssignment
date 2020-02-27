using Microsoft.EntityFrameworkCore.Migrations;

namespace YangdoDAO.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalHours",
                schema: "dbo",
                table: "TimeSheets",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalHours",
                schema: "dbo",
                table: "TimeSheets");
        }
    }
}
