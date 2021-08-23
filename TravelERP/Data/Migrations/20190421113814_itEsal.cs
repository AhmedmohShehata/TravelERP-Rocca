using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class itEsal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Esals_MenuLE0_MenuLE0Id",
                table: "Esals");

            migrationBuilder.DropColumn(
                name: "MenuLE1Id",
                table: "Esals");

            migrationBuilder.AlterColumn<int>(
                name: "MenuLE0Id",
                table: "Esals",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Esals_MenuLE0_MenuLE0Id",
                table: "Esals",
                column: "MenuLE0Id",
                principalTable: "MenuLE0",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Esals_MenuLE0_MenuLE0Id",
                table: "Esals");

            migrationBuilder.AlterColumn<int>(
                name: "MenuLE0Id",
                table: "Esals",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "MenuLE1Id",
                table: "Esals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Esals_MenuLE0_MenuLE0Id",
                table: "Esals",
                column: "MenuLE0Id",
                principalTable: "MenuLE0",
                principalColumn: "Id");
        }
    }
}
