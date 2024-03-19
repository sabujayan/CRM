using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Indo.Migrations
{
    public partial class modifiedEmployeeEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppEmployeeClientMatrices_AppClientses_ClientsId",
                table: "AppEmployeeClientMatrices");

            migrationBuilder.DropForeignKey(
                name: "FK_AppEmployeeClientMatrices_AppEmployees_EmployeeId",
                table: "AppEmployeeClientMatrices");

            migrationBuilder.DropForeignKey(
                name: "FK_AppEmployeeProjectMatrices_AppEmployees_EmployeeId",
                table: "AppEmployeeProjectMatrices");

            migrationBuilder.DropForeignKey(
                name: "FK_AppEmployeeProjectMatrices_AppProject_ProjectsId",
                table: "AppEmployeeProjectMatrices");

            migrationBuilder.DropForeignKey(
                name: "FK_AppEmployees_AppEmployeeSkillMatricess_EmployeeSkillMatricesId",
                table: "AppEmployees");

            migrationBuilder.DropIndex(
                name: "IX_AppEmployees_EmployeeSkillMatricesId",
                table: "AppEmployees");

            migrationBuilder.DropIndex(
                name: "IX_AppEmployeeProjectMatrices_EmployeeId",
                table: "AppEmployeeProjectMatrices");

            migrationBuilder.DropIndex(
                name: "IX_AppEmployeeProjectMatrices_ProjectsId",
                table: "AppEmployeeProjectMatrices");

            migrationBuilder.DropIndex(
                name: "IX_AppEmployeeClientMatrices_ClientsId",
                table: "AppEmployeeClientMatrices");

            migrationBuilder.DropIndex(
                name: "IX_AppEmployeeClientMatrices_EmployeeId",
                table: "AppEmployeeClientMatrices");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "AppEmployeeSkillMatricess",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "AppEmployeeSkillMatricess");

            migrationBuilder.CreateIndex(
                name: "IX_AppEmployees_EmployeeSkillMatricesId",
                table: "AppEmployees",
                column: "EmployeeSkillMatricesId");

            migrationBuilder.CreateIndex(
                name: "IX_AppEmployeeProjectMatrices_EmployeeId",
                table: "AppEmployeeProjectMatrices",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppEmployeeProjectMatrices_ProjectsId",
                table: "AppEmployeeProjectMatrices",
                column: "ProjectsId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AppEmployeeProjectMatrices_AppEmployees_EmployeeId",
                table: "AppEmployeeProjectMatrices",
                column: "EmployeeId",
                principalTable: "AppEmployees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppEmployeeProjectMatrices_AppProject_ProjectsId",
                table: "AppEmployeeProjectMatrices",
                column: "ProjectsId",
                principalTable: "AppProject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppEmployees_AppEmployeeSkillMatricess_EmployeeSkillMatricesId",
                table: "AppEmployees",
                column: "EmployeeSkillMatricesId",
                principalTable: "AppEmployeeSkillMatricess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
