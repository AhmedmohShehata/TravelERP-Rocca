using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class AddCompanyIdtoSalary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "salaryAddandCuts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_salaryAddandCuts_CompanyId",
                table: "salaryAddandCuts",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_salaryAddandCuts_Companies_CompanyId",
                table: "salaryAddandCuts",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_salaryAddandCuts_Companies_CompanyId",
                table: "salaryAddandCuts");

            migrationBuilder.DropIndex(
                name: "IX_salaryAddandCuts_CompanyId",
                table: "salaryAddandCuts");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "salaryAddandCuts");
        }
    }
}
