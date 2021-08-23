using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class Personnel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EznPersonnels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AbsenceVal = table.Column<int>(nullable: false),
                    Allowances = table.Column<int>(nullable: false),
                    BasicSalary = table.Column<int>(nullable: false),
                    Commissions = table.Column<int>(nullable: false),
                    Incentives = table.Column<int>(nullable: false),
                    Insurance = table.Column<int>(nullable: false),
                    LateVal = table.Column<int>(nullable: false),
                    Loans = table.Column<int>(nullable: false),
                    OverVal = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EznPersonnels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EznPersonnels_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Personnels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AbsenceHrVal = table.Column<int>(nullable: false),
                    Allowances = table.Column<int>(nullable: false),
                    BasicSalary = table.Column<int>(nullable: false),
                    Commissions = table.Column<int>(nullable: false),
                    FingerPrintName = table.Column<int>(nullable: true),
                    Incentives = table.Column<int>(nullable: false),
                    Insurance = table.Column<int>(nullable: false),
                    LateHrVal = table.Column<int>(nullable: false),
                    OverHrVal = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personnels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Personnels_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EznPersonnels_UserId",
                table: "EznPersonnels",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Personnels_UserId",
                table: "Personnels",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EznPersonnels");

            migrationBuilder.DropTable(
                name: "Personnels");
        }
    }
}
