using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Indo.Migrations
{
    public partial class AddAccountsInfoEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountsInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountSite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentAccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnnualRevenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TickerSymbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ownership = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Employees = table.Column<int>(type: "int", nullable: false),
                    SICCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillingStreet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillingCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillingState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillingCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillingCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingStreet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_AccountsInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountsInfo_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountsInfo_UserId",
                table: "AccountsInfo",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountsInfo");
        }
    }
}
