using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Indo.Migrations
{
    public partial class Employee_client_modify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ClientsId",
                table: "AppEmployeeClientMatrices",
                type: "uniqueidentifier",
                maxLength: 250000,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 250000,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "AppEmployeeClientMatrices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "AppEmployeeClientMatrices");

            migrationBuilder.AlterColumn<string>(
                name: "ClientsId",
                table: "AppEmployeeClientMatrices",
                type: "nvarchar(max)",
                maxLength: 250000,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldMaxLength: 250000);
        }
    }
}
