using Microsoft.EntityFrameworkCore.Migrations;

namespace Indo.Migrations
{
    public partial class _rmv_clientid_Employee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientID",
                table: "AppEmployees");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientID",
                table: "AppEmployees",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
