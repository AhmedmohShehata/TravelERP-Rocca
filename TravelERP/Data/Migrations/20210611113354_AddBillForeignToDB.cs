using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class AddBillForeignToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "billForeigns",
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
                    EMPCommission = table.Column<float>(nullable: false),
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
                    table.PrimaryKey("PK_billForeigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_billForeigns_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_billForeigns_CustomerOrSuppliers_CustomerOrSupplierId",
                        column: x => x.CustomerOrSupplierId,
                        principalTable: "CustomerOrSuppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_billForeigns_CustomersSuppliers_CustomerSupplierId",
                        column: x => x.CustomerSupplierId,
                        principalTable: "CustomersSuppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_billForeigns_MenuLE0_MenuLE0Id",
                        column: x => x.MenuLE0Id,
                        principalTable: "MenuLE0",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_billForeigns_MenuLE1_MenuLE1Id",
                        column: x => x.MenuLE1Id,
                        principalTable: "MenuLE1",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_billForeigns_MenuLE2_MenuLE2Id",
                        column: x => x.MenuLE2Id,
                        principalTable: "MenuLE2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_billForeigns_CustomersSuppliers_TicketExportId",
                        column: x => x.TicketExportId,
                        principalTable: "CustomersSuppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_billForeigns_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_billForeigns_CompanyID",
                table: "billForeigns",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_billForeigns_CustomerOrSupplierId",
                table: "billForeigns",
                column: "CustomerOrSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_billForeigns_CustomerSupplierId",
                table: "billForeigns",
                column: "CustomerSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_billForeigns_MenuLE0Id",
                table: "billForeigns",
                column: "MenuLE0Id");

            migrationBuilder.CreateIndex(
                name: "IX_billForeigns_MenuLE1Id",
                table: "billForeigns",
                column: "MenuLE1Id");

            migrationBuilder.CreateIndex(
                name: "IX_billForeigns_MenuLE2Id",
                table: "billForeigns",
                column: "MenuLE2Id");

            migrationBuilder.CreateIndex(
                name: "IX_billForeigns_TicketExportId",
                table: "billForeigns",
                column: "TicketExportId");

            migrationBuilder.CreateIndex(
                name: "IX_billForeigns_UserId",
                table: "billForeigns",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "billForeigns");
        }
    }
}
