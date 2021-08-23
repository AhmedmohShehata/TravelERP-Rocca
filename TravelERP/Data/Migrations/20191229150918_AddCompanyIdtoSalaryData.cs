using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class AddCompanyIdtoSalaryData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "salaryDatas",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_salaryDatas_CompanyId",
                table: "salaryDatas",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_salaryDatas_Companies_CompanyId",
                table: "salaryDatas",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_salaryDatas_Companies_CompanyId",
                table: "salaryDatas");

            migrationBuilder.DropIndex(
                name: "IX_salaryDatas_CompanyId",
                table: "salaryDatas");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "salaryDatas");
        }
    }
}
