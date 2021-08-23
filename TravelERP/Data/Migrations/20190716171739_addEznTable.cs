using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class addEznTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ezns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AmountWithdrawn = table.Column<int>(nullable: false),
                    CompanyID = table.Column<int>(nullable: false),
                    ExpenseName = table.Column<string>(nullable: true),
                    EznDate = table.Column<DateTime>(nullable: false),
                    EznId = table.Column<int>(nullable: false),
                    MenuLZ0Id = table.Column<int>(nullable: false),
                    MenuLZ1Id = table.Column<int>(nullable: false),
                    MenuLZ2Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PaymentMethodId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ezns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ezns_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ezns_MenuLZ0_MenuLZ0Id",
                        column: x => x.MenuLZ0Id,
                        principalTable: "MenuLZ0",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ezns_MenuLZ1_MenuLZ1Id",
                        column: x => x.MenuLZ1Id,
                        principalTable: "MenuLZ1",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ezns_MenuLZ2_MenuLZ2Id",
                        column: x => x.MenuLZ2Id,
                        principalTable: "MenuLZ2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ezns_paymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "paymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ezns_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ezns_CompanyID",
                table: "Ezns",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Ezns_MenuLZ0Id",
                table: "Ezns",
                column: "MenuLZ0Id");

            migrationBuilder.CreateIndex(
                name: "IX_Ezns_MenuLZ1Id",
                table: "Ezns",
                column: "MenuLZ1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Ezns_MenuLZ2Id",
                table: "Ezns",
                column: "MenuLZ2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Ezns_PaymentMethodId",
                table: "Ezns",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Ezns_UserId",
                table: "Ezns",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ezns");
        }
    }
}
