using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class addEznForEsalTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EznsForEsals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AmountWithdrawn = table.Column<int>(nullable: false),
                    CompanyID = table.Column<int>(nullable: false),
                    ExpenseName = table.Column<string>(nullable: true),
                    EznDate = table.Column<DateTime>(nullable: false),
                    EznId = table.Column<int>(nullable: false),
                    MenuLE0Id = table.Column<int>(nullable: false),
                    MenuLE1Id = table.Column<int>(nullable: false),
                    MenuLE2Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PaymentMethodId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EznsForEsals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EznsForEsals_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EznsForEsals_MenuLE0_MenuLE0Id",
                        column: x => x.MenuLE0Id,
                        principalTable: "MenuLE0",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EznsForEsals_MenuLE1_MenuLE1Id",
                        column: x => x.MenuLE1Id,
                        principalTable: "MenuLE1",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EznsForEsals_MenuLE2_MenuLE2Id",
                        column: x => x.MenuLE2Id,
                        principalTable: "MenuLE2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EznsForEsals_paymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "paymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EznsForEsals_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EznsForEsals_CompanyID",
                table: "EznsForEsals",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_EznsForEsals_MenuLE0Id",
                table: "EznsForEsals",
                column: "MenuLE0Id");

            migrationBuilder.CreateIndex(
                name: "IX_EznsForEsals_MenuLE1Id",
                table: "EznsForEsals",
                column: "MenuLE1Id");

            migrationBuilder.CreateIndex(
                name: "IX_EznsForEsals_MenuLE2Id",
                table: "EznsForEsals",
                column: "MenuLE2Id");

            migrationBuilder.CreateIndex(
                name: "IX_EznsForEsals_PaymentMethodId",
                table: "EznsForEsals",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_EznsForEsals_UserId",
                table: "EznsForEsals",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EznsForEsals");
        }
    }
}
