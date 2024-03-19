using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Indo.Migrations
{
    public partial class ModifiedTecnologyEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AppTechnology",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "AppTechnology",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppTechnology_ParentId",
                table: "AppTechnology",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppTechnology_AppTechnology_ParentId",
                table: "AppTechnology",
                column: "ParentId",
                principalTable: "AppTechnology",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppTechnology_AppTechnology_ParentId",
                table: "AppTechnology");

            migrationBuilder.DropIndex(
                name: "IX_AppTechnology_ParentId",
                table: "AppTechnology");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AppTechnology");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "AppTechnology");
        }
    }
}
