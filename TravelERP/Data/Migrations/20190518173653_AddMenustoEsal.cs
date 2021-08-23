using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class AddMenustoEsal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuLE1Id",
                table: "Esals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MenuLE2Id",
                table: "Esals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Esals_MenuLE1Id",
                table: "Esals",
                column: "MenuLE1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Esals_MenuLE2Id",
                table: "Esals",
                column: "MenuLE2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Esals_MenuLE1_MenuLE1Id",
                table: "Esals",
                column: "MenuLE1Id",
                principalTable: "MenuLE1",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Esals_MenuLE2_MenuLE2Id",
                table: "Esals",
                column: "MenuLE2Id",
                principalTable: "MenuLE2",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Esals_MenuLE1_MenuLE1Id",
                table: "Esals");

            migrationBuilder.DropForeignKey(
                name: "FK_Esals_MenuLE2_MenuLE2Id",
                table: "Esals");

            migrationBuilder.DropIndex(
                name: "IX_Esals_MenuLE1Id",
                table: "Esals");

            migrationBuilder.DropIndex(
                name: "IX_Esals_MenuLE2Id",
                table: "Esals");

            migrationBuilder.DropColumn(
                name: "MenuLE1Id",
                table: "Esals");

            migrationBuilder.DropColumn(
                name: "MenuLE2Id",
                table: "Esals");
        }
    }
}
