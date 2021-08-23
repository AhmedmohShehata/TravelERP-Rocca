using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class EsalEditMenus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Esals_MenuLE1_MenuLE1Id",
                table: "Esals");

            migrationBuilder.DropForeignKey(
                name: "FK_Esals_MenuLE2_MenuLE2Id",
                table: "Esals");

            migrationBuilder.AlterColumn<int>(
                name: "MenuLE2Id",
                table: "Esals",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "MenuLE1Id",
                table: "Esals",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Esals_MenuLE1_MenuLE1Id",
                table: "Esals",
                column: "MenuLE1Id",
                principalTable: "MenuLE1",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Esals_MenuLE2_MenuLE2Id",
                table: "Esals",
                column: "MenuLE2Id",
                principalTable: "MenuLE2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Esals_MenuLE1_MenuLE1Id",
                table: "Esals");

            migrationBuilder.DropForeignKey(
                name: "FK_Esals_MenuLE2_MenuLE2Id",
                table: "Esals");

            migrationBuilder.AlterColumn<int>(
                name: "MenuLE2Id",
                table: "Esals",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MenuLE1Id",
                table: "Esals",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Esals_MenuLE1_MenuLE1Id",
                table: "Esals",
                column: "MenuLE1Id",
                principalTable: "MenuLE1",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Esals_MenuLE2_MenuLE2Id",
                table: "Esals",
                column: "MenuLE2Id",
                principalTable: "MenuLE2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
