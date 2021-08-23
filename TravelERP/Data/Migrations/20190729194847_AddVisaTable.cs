using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class AddVisaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Visas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AdultN = table.Column<int>(nullable: false),
                    BillDate = table.Column<DateTime>(nullable: false),
                    BillId = table.Column<int>(nullable: false),
                    ChildN = table.Column<int>(nullable: false),
                    Commnets = table.Column<string>(nullable: true),
                    CompanyID = table.Column<int>(nullable: false),
                    CustomerOrSupplierId = table.Column<int>(nullable: false),
                    CustomerPrice = table.Column<int>(nullable: false),
                    CustomerSupplierId = table.Column<int>(nullable: false),
                    MenuLE0Id = table.Column<int>(nullable: false),
                    MenuLE1Id = table.Column<int>(nullable: false),
                    MenuLE2Id = table.Column<int>(nullable: false),
                    NetPrice = table.Column<int>(nullable: false),
                    TicketExportId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Visas_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Visas_CustomerOrSuppliers_CustomerOrSupplierId",
                        column: x => x.CustomerOrSupplierId,
                        principalTable: "CustomerOrSuppliers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Visas_CustomersSuppliers_CustomerSupplierId",
                        column: x => x.CustomerSupplierId,
                        principalTable: "CustomersSuppliers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Visas_MenuLE0_MenuLE0Id",
                        column: x => x.MenuLE0Id,
                        principalTable: "MenuLE0",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Visas_MenuLE1_MenuLE1Id",
                        column: x => x.MenuLE1Id,
                        principalTable: "MenuLE1",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Visas_MenuLE2_MenuLE2Id",
                        column: x => x.MenuLE2Id,
                        principalTable: "MenuLE2",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Visas_CustomersSuppliers_TicketExportId",
                        column: x => x.TicketExportId,
                        principalTable: "CustomersSuppliers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Visas_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Visas_CompanyID",
                table: "Visas",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Visas_CustomerOrSupplierId",
                table: "Visas",
                column: "CustomerOrSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Visas_CustomerSupplierId",
                table: "Visas",
                column: "CustomerSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Visas_MenuLE0Id",
                table: "Visas",
                column: "MenuLE0Id");

            migrationBuilder.CreateIndex(
                name: "IX_Visas_MenuLE1Id",
                table: "Visas",
                column: "MenuLE1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Visas_MenuLE2Id",
                table: "Visas",
                column: "MenuLE2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Visas_TicketExportId",
                table: "Visas",
                column: "TicketExportId");

            migrationBuilder.CreateIndex(
                name: "IX_Visas_UserId",
                table: "Visas",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Visas");
        }
    }
}
