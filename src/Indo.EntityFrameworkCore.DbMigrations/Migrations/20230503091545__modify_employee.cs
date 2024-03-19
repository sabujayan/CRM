using Microsoft.EntityFrameworkCore.Migrations;

namespace Indo.Migrations
{
    public partial class _modify_employee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectID",
                table: "AppEmployees");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectID",
                table: "AppEmployees",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
