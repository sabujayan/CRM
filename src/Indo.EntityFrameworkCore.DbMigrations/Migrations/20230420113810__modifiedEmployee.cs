using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Indo.Migrations
{
    public partial class _modifiedEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bandwidth",
                table: "AppEmployees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientID",
                table: "AppEmployees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeSkillMatricesId",
                table: "AppEmployees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ProjectID",
                table: "AppEmployees",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bandwidth",
                table: "AppEmployees");

            migrationBuilder.DropColumn(
                name: "ClientID",
                table: "AppEmployees");

            migrationBuilder.DropColumn(
                name: "EmployeeSkillMatricesId",
                table: "AppEmployees");

            migrationBuilder.DropColumn(
                name: "ProjectID",
                table: "AppEmployees");
        }
    }
}
