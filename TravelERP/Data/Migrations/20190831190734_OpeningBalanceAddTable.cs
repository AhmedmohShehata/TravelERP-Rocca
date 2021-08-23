using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class OpeningBalanceAddTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OpeningBalances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Balance = table.Column<int>(nullable: false),
                    CustomerOrSupplierId = table.Column<int>(nullable: false),
                    CustomerSupplierId = table.Column<int>(nullable: false),
                    MenuLE0Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpeningBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpeningBalances_CustomerOrSuppliers_CustomerOrSupplierId",
                        column: x => x.CustomerOrSupplierId,
                        principalTable: "CustomerOrSuppliers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OpeningBalances_CustomersSuppliers_CustomerSupplierId",
                        column: x => x.CustomerSupplierId,
                        principalTable: "CustomersSuppliers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OpeningBalances_MenuLE0_MenuLE0Id",
                        column: x => x.MenuLE0Id,
                        principalTable: "MenuLE0",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpeningBalances_CustomerOrSupplierId",
                table: "OpeningBalances",
                column: "CustomerOrSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_OpeningBalances_CustomerSupplierId",
                table: "OpeningBalances",
                column: "CustomerSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_OpeningBalances_MenuLE0Id",
                table: "OpeningBalances",
                column: "MenuLE0Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpeningBalances");
        }
    }
}
