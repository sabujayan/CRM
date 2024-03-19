using Microsoft.EntityFrameworkCore.Migrations;

namespace Indo.Migrations
{
    public partial class _added_modifyEstimate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estimate",
                table: "AppProject");

            migrationBuilder.AddColumn<float>(
                name: "EstimateHours",
                table: "AppProject",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimateHours",
                table: "AppProject");

            migrationBuilder.AddColumn<long>(
                name: "Estimate",
                table: "AppProject",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
