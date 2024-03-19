using Microsoft.EntityFrameworkCore.Migrations;

namespace Indo.Migrations
{
    public partial class _AddedemployeeSkillMigrationmodified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppEmployeeSkillMatricess_DepartmentId",
                table: "AppEmployeeSkillMatricess",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppEmployeeSkillMatricess_SkillId",
                table: "AppEmployeeSkillMatricess",
                column: "SkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppEmployeeSkillMatricess_AppDepartments_DepartmentId",
                table: "AppEmployeeSkillMatricess",
                column: "DepartmentId",
                principalTable: "AppDepartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppEmployeeSkillMatricess_AppSkills_SkillId",
                table: "AppEmployeeSkillMatricess",
                column: "SkillId",
                principalTable: "AppSkills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppEmployeeSkillMatricess_AppDepartments_DepartmentId",
                table: "AppEmployeeSkillMatricess");

            migrationBuilder.DropForeignKey(
                name: "FK_AppEmployeeSkillMatricess_AppSkills_SkillId",
                table: "AppEmployeeSkillMatricess");

            migrationBuilder.DropIndex(
                name: "IX_AppEmployeeSkillMatricess_DepartmentId",
                table: "AppEmployeeSkillMatricess");

            migrationBuilder.DropIndex(
                name: "IX_AppEmployeeSkillMatricess_SkillId",
                table: "AppEmployeeSkillMatricess");
        }
    }
}
