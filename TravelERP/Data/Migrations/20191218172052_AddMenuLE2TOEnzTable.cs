using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class AddMenuLE2TOEnzTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuLE2Id",
                table: "Ezns",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ezns_MenuLE2Id",
                table: "Ezns",
                column: "MenuLE2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_MenuLE2_MenuLE2Id",
                table: "Ezns",
                column: "MenuLE2Id",
                principalTable: "MenuLE2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_MenuLE2_MenuLE2Id",
                table: "Ezns");

            migrationBuilder.DropIndex(
                name: "IX_Ezns_MenuLE2Id",
                table: "Ezns");

            migrationBuilder.DropColumn(
                name: "MenuLE2Id",
                table: "Ezns");
        }
    }
}
