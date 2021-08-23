using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class AddMenuLE1EznTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuLE1Id",
                table: "Ezns",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ezns_MenuLE1Id",
                table: "Ezns",
                column: "MenuLE1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_MenuLE0_MenuLE1Id",
                table: "Ezns",
                column: "MenuLE1Id",
                principalTable: "MenuLE0",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_MenuLE0_MenuLE1Id",
                table: "Ezns");

            migrationBuilder.DropIndex(
                name: "IX_Ezns_MenuLE1Id",
                table: "Ezns");

            migrationBuilder.DropColumn(
                name: "MenuLE1Id",
                table: "Ezns");
        }
    }
}
