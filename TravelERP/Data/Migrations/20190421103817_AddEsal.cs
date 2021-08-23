using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class AddEsal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Esals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AmountPaid = table.Column<int>(nullable: false),
                    BillId = table.Column<int>(nullable: false),
                    EsalId = table.Column<int>(nullable: false),
                    MenuLE0Id = table.Column<int>(nullable: true),
                    MenuLE1Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Esals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Esals_BillAirLines_BillId",
                        column: x => x.BillId,
                        principalTable: "BillAirLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Esals_MenuLE0_MenuLE0Id",
                        column: x => x.MenuLE0Id,
                        principalTable: "MenuLE0",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Esals_BillId",
                table: "Esals",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_Esals_MenuLE0Id",
                table: "Esals",
                column: "MenuLE0Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Esals");
        }
    }
}
