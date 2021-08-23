using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class MEnuLZ2CompanyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyID",
                table: "MenuLZ2",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MenuLZ2_CompanyID",
                table: "MenuLZ2",
                column: "CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuLZ2_Companies_CompanyID",
                table: "MenuLZ2",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuLZ2_Companies_CompanyID",
                table: "MenuLZ2");

            migrationBuilder.DropIndex(
                name: "IX_MenuLZ2_CompanyID",
                table: "MenuLZ2");

            migrationBuilder.DropColumn(
                name: "CompanyID",
                table: "MenuLZ2");
        }
    }
}
