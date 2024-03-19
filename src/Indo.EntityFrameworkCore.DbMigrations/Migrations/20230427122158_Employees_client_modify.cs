using Microsoft.EntityFrameworkCore.Migrations;

namespace Indo.Migrations
{
    public partial class Employees_client_modify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppEmployeeClientMatrices_ClientsId",
                table: "AppEmployeeClientMatrices",
                column: "ClientsId");

            migrationBuilder.CreateIndex(
                name: "IX_AppEmployeeClientMatrices_EmployeeId",
                table: "AppEmployeeClientMatrices",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppEmployeeClientMatrices_AppClientses_ClientsId",
                table: "AppEmployeeClientMatrices",
                column: "ClientsId",
                principalTable: "AppClientses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppEmployeeClientMatrices_AppEmployees_EmployeeId",
                table: "AppEmployeeClientMatrices",
                column: "EmployeeId",
                principalTable: "AppEmployees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppEmployeeClientMatrices_AppClientses_ClientsId",
                table: "AppEmployeeClientMatrices");

            migrationBuilder.DropForeignKey(
                name: "FK_AppEmployeeClientMatrices_AppEmployees_EmployeeId",
                table: "AppEmployeeClientMatrices");

            migrationBuilder.DropIndex(
                name: "IX_AppEmployeeClientMatrices_ClientsId",
                table: "AppEmployeeClientMatrices");

            migrationBuilder.DropIndex(
                name: "IX_AppEmployeeClientMatrices_EmployeeId",
                table: "AppEmployeeClientMatrices");
        }
    }
}
