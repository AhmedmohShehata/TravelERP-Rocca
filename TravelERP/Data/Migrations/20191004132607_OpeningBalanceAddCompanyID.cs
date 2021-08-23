using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class OpeningBalanceAddCompanyID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyID",
                table: "OpeningBalances",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OpeningBalances_CompanyID",
                table: "OpeningBalances",
                column: "CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningBalances_Companies_CompanyID",
                table: "OpeningBalances",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpeningBalances_Companies_CompanyID",
                table: "OpeningBalances");

            migrationBuilder.DropIndex(
                name: "IX_OpeningBalances_CompanyID",
                table: "OpeningBalances");

            migrationBuilder.DropColumn(
                name: "CompanyID",
                table: "OpeningBalances");
        }
    }
}
