using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Indo.Migrations
{
    public partial class _added_projecttechnology : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppProjectTechnologyMatrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TechnologyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_AppProjectTechnologyMatrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppProjectTechnologyMatrices_AppProject_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "AppProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppProjectTechnologyMatrices_AppTechnology_TechnologyId",
                        column: x => x.TechnologyId,
                        principalTable: "AppTechnology",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppProjectTechnologyMatrices_ProjectsId",
                table: "AppProjectTechnologyMatrices",
                column: "ProjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProjectTechnologyMatrices_TechnologyId",
                table: "AppProjectTechnologyMatrices",
                column: "TechnologyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppProjectTechnologyMatrices");
        }
    }
}
