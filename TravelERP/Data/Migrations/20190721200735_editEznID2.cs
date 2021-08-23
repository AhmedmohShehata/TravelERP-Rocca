using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class editEznID2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_Companies_CompanyId",
                table: "Ezns");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Ezns",
                newName: "CompanyID");

            migrationBuilder.RenameIndex(
                name: "IX_Ezns_CompanyId",
                table: "Ezns",
                newName: "IX_Ezns_CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_Companies_CompanyID",
                table: "Ezns",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_Companies_CompanyID",
                table: "Ezns");

            migrationBuilder.RenameColumn(
                name: "CompanyID",
                table: "Ezns",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Ezns_CompanyID",
                table: "Ezns",
                newName: "IX_Ezns_CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_Companies_CompanyId",
                table: "Ezns",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
