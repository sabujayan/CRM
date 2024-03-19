using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Indo.Migrations
{
    public partial class _modifiedEmployeedSkillMatrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "AppEmployeeSkillMatricess");

            migrationBuilder.DropColumn(
                name: "SkillId",
                table: "AppEmployeeSkillMatricess");

            migrationBuilder.AddColumn<string>(
                name: "SkillsId",
                table: "AppEmployeeSkillMatricess",
                type: "nvarchar(max)",
                maxLength: 250000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SkillsId",
                table: "AppEmployeeSkillMatricess");

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "AppEmployeeSkillMatricess",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SkillId",
                table: "AppEmployeeSkillMatricess",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
    }
}
