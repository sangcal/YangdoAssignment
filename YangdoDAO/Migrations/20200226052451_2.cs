using Microsoft.EntityFrameworkCore.Migrations;

namespace YangdoDAO.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSheets_People_PersonId",
                schema: "dbo",
                table: "TimeSheets");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeSheets_Tasks_TaskId",
                schema: "dbo",
                table: "TimeSheets");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSheets_People_PersonId",
                schema: "dbo",
                table: "TimeSheets",
                column: "PersonId",
                principalSchema: "dbo",
                principalTable: "People",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSheets_Tasks_TaskId",
                schema: "dbo",
                table: "TimeSheets",
                column: "TaskId",
                principalSchema: "dbo",
                principalTable: "Tasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSheets_People_PersonId",
                schema: "dbo",
                table: "TimeSheets");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeSheets_Tasks_TaskId",
                schema: "dbo",
                table: "TimeSheets");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSheets_People_PersonId",
                schema: "dbo",
                table: "TimeSheets",
                column: "PersonId",
                principalSchema: "dbo",
                principalTable: "People",
                principalColumn: "PersonId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSheets_Tasks_TaskId",
                schema: "dbo",
                table: "TimeSheets",
                column: "TaskId",
                principalSchema: "dbo",
                principalTable: "Tasks",
                principalColumn: "TaskId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
