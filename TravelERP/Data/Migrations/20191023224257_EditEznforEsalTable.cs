using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class EditEznforEsalTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EznsForEsals_MenuLE1_MenuLE1Id",
                table: "EznsForEsals");

            migrationBuilder.DropForeignKey(
                name: "FK_EznsForEsals_MenuLE2_MenuLE2Id",
                table: "EznsForEsals");

            migrationBuilder.DropIndex(
                name: "IX_EznsForEsals_MenuLE1Id",
                table: "EznsForEsals");

            migrationBuilder.DropIndex(
                name: "IX_EznsForEsals_MenuLE2Id",
                table: "EznsForEsals");

            migrationBuilder.DropColumn(
                name: "MenuLE1Id",
                table: "EznsForEsals");

            migrationBuilder.DropColumn(
                name: "MenuLE2Id",
                table: "EznsForEsals");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuLE1Id",
                table: "EznsForEsals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MenuLE2Id",
                table: "EznsForEsals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EznsForEsals_MenuLE1Id",
                table: "EznsForEsals",
                column: "MenuLE1Id");

            migrationBuilder.CreateIndex(
                name: "IX_EznsForEsals_MenuLE2Id",
                table: "EznsForEsals",
                column: "MenuLE2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EznsForEsals_MenuLE1_MenuLE1Id",
                table: "EznsForEsals",
                column: "MenuLE1Id",
                principalTable: "MenuLE1",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EznsForEsals_MenuLE2_MenuLE2Id",
                table: "EznsForEsals",
                column: "MenuLE2Id",
                principalTable: "MenuLE2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
