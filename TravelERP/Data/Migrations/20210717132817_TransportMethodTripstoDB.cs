using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class TransportMethodTripstoDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransportMethodTrips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillId = table.Column<int>(nullable: false),
                    BillIdId = table.Column<int>(nullable: false),
                    IsBus = table.Column<bool>(nullable: false),
                    MenuLE0Id = table.Column<int>(nullable: false),
                    MenuLE1Id = table.Column<int>(nullable: false),
                    MenuLE2Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportMethodTrips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportMethodTrips_MenuLE0_MenuLE0Id",
                        column: x => x.MenuLE0Id,
                        principalTable: "MenuLE0",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransportMethodTrips_MenuLE1_MenuLE1Id",
                        column: x => x.MenuLE1Id,
                        principalTable: "MenuLE1",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransportMethodTrips_MenuLE2_MenuLE2Id",
                        column: x => x.MenuLE2Id,
                        principalTable: "MenuLE2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransportMethodTrips_MenuLE0Id",
                table: "TransportMethodTrips",
                column: "MenuLE0Id");

            migrationBuilder.CreateIndex(
                name: "IX_TransportMethodTrips_MenuLE1Id",
                table: "TransportMethodTrips",
                column: "MenuLE1Id");

            migrationBuilder.CreateIndex(
                name: "IX_TransportMethodTrips_MenuLE2Id",
                table: "TransportMethodTrips",
                column: "MenuLE2Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransportMethodTrips");
        }
    }
}
