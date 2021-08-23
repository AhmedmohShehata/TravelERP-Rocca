using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class addCompanyIdToSalaryAttendanceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "salaryAttendances",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_salaryAttendances_CompanyId",
                table: "salaryAttendances",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_salaryAttendances_Companies_CompanyId",
                table: "salaryAttendances",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_salaryAttendances_Companies_CompanyId",
                table: "salaryAttendances");

            migrationBuilder.DropIndex(
                name: "IX_salaryAttendances_CompanyId",
                table: "salaryAttendances");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "salaryAttendances");
        }
    }
}
