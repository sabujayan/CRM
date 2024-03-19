using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Indo.Migrations
{
    public partial class _added_client_project : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppClientProjectMatrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppClientProjectMatrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppClientProjectMatrices_AppClientses_ClientsId",
                        column: x => x.ClientsId,
                        principalTable: "AppClientses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AppClientProjectMatrices_AppProject_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "AppProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppClientProjectMatrices_ClientsId",
                table: "AppClientProjectMatrices",
                column: "ClientsId");

            migrationBuilder.CreateIndex(
                name: "IX_AppClientProjectMatrices_ProjectsId",
                table: "AppClientProjectMatrices",
                column: "ProjectsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppClientProjectMatrices");
        }
    }
}
