using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class BillDomestic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BillDomestic",
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
                    Direction = table.Column<string>(nullable: true),
                    MenuLE0Id = table.Column<int>(nullable: false),
                    MenuLE1Id = table.Column<int>(nullable: false),
                    MenuLE2Id = table.Column<int>(nullable: false),
                    NetPrice = table.Column<int>(nullable: false),
                    TicketExportId = table.Column<int>(nullable: false),
                    TicketFrom = table.Column<DateTime>(nullable: false),
                    TicketTo = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillDomestic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillDomestic_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillDomestic_CustomerOrSuppliers_CustomerOrSupplierId",
                        column: x => x.CustomerOrSupplierId,
                        principalTable: "CustomerOrSuppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillDomestic_CustomersSuppliers_CustomerSupplierId",
                        column: x => x.CustomerSupplierId,
                        principalTable: "CustomersSuppliers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BillDomestic_MenuLE0_MenuLE0Id",
                        column: x => x.MenuLE0Id,
                        principalTable: "MenuLE0",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillDomestic_MenuLE1_MenuLE1Id",
                        column: x => x.MenuLE1Id,
                        principalTable: "MenuLE1",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillDomestic_MenuLE2_MenuLE2Id",
                        column: x => x.MenuLE2Id,
                        principalTable: "MenuLE2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillDomestic_CustomersSuppliers_TicketExportId",
                        column: x => x.TicketExportId,
                        principalTable: "CustomersSuppliers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BillDomestic_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BillDomestic_CompanyID",
                table: "BillDomestic",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_BillDomestic_CustomerOrSupplierId",
                table: "BillDomestic",
                column: "CustomerOrSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_BillDomestic_CustomerSupplierId",
                table: "BillDomestic",
                column: "CustomerSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_BillDomestic_MenuLE0Id",
                table: "BillDomestic",
                column: "MenuLE0Id");

            migrationBuilder.CreateIndex(
                name: "IX_BillDomestic_MenuLE1Id",
                table: "BillDomestic",
                column: "MenuLE1Id");

            migrationBuilder.CreateIndex(
                name: "IX_BillDomestic_MenuLE2Id",
                table: "BillDomestic",
                column: "MenuLE2Id");

            migrationBuilder.CreateIndex(
                name: "IX_BillDomestic_TicketExportId",
                table: "BillDomestic",
                column: "TicketExportId");

            migrationBuilder.CreateIndex(
                name: "IX_BillDomestic_UserId",
                table: "BillDomestic",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillDomestic");
        }
    }
}
