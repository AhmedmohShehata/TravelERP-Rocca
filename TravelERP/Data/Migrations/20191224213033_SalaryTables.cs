using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class SalaryTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "salaryAddandCuts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Bonus_Add = table.Column<int>(nullable: false),
                    EMP_Name = table.Column<string>(nullable: true),
                    FromDate = table.Column<DateTime>(nullable: false),
                    Omra_Add = table.Column<int>(nullable: false),
                    Other_Cut = table.Column<int>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    Whatsapp_Cut = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salaryAddandCuts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "salaryDatas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EMP_Absent = table.Column<int>(nullable: false),
                    EMP_Early = table.Column<int>(nullable: false),
                    EMP_FingerPrintName = table.Column<string>(nullable: true),
                    EMP_Late = table.Column<int>(nullable: false),
                    EMP_Name = table.Column<string>(nullable: true),
                    EMP_OverTime = table.Column<int>(nullable: false),
                    EMP_Salary = table.Column<int>(nullable: false),
                    EMP_WhatsApp = table.Column<int>(nullable: false),
                    EMP_insurance = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salaryDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_salaryDatas_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_salaryDatas_UserId",
                table: "salaryDatas",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "salaryAddandCuts");

            migrationBuilder.DropTable(
                name: "salaryDatas");
        }
    }
}
