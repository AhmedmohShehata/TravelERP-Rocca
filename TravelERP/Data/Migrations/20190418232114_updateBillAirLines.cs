using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class updateBillAirLines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BillAirLines_CompanyID",
                table: "BillAirLines",
                column: "CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_BillAirLines_Companies_CompanyID",
                table: "BillAirLines",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillAirLines_Companies_CompanyID",
                table: "BillAirLines");

            migrationBuilder.DropIndex(
                name: "IX_BillAirLines_CompanyID",
                table: "BillAirLines");
        }
    }
}
