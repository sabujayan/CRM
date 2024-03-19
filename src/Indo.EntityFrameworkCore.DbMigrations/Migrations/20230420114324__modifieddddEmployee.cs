using Microsoft.EntityFrameworkCore.Migrations;

namespace Indo.Migrations
{
    public partial class _modifieddddEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppEmployees_EmployeeSkillMatricesId",
                table: "AppEmployees",
                column: "EmployeeSkillMatricesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppEmployees_AppEmployeeSkillMatricess_EmployeeSkillMatricesId",
                table: "AppEmployees",
                column: "EmployeeSkillMatricesId",
                principalTable: "AppEmployeeSkillMatricess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppEmployees_AppEmployeeSkillMatricess_EmployeeSkillMatricesId",
                table: "AppEmployees");

            migrationBuilder.DropIndex(
                name: "IX_AppEmployees_EmployeeSkillMatricesId",
                table: "AppEmployees");
        }
    }
}
