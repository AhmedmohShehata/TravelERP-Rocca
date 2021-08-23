using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class AddUtility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillReligious_MenuLE2_MenuLE2Id",
                table: "BillReligious");

            migrationBuilder.AddColumn<int>(
                name: "Name5",
                table: "DatesSearches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Name6",
                table: "DatesSearches",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "MenuLE2Id",
                table: "BillReligious",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BillReligious_MenuLE2_MenuLE2Id",
                table: "BillReligious",
                column: "MenuLE2Id",
                principalTable: "MenuLE2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillReligious_MenuLE2_MenuLE2Id",
                table: "BillReligious");

            migrationBuilder.DropColumn(
                name: "Name5",
                table: "DatesSearches");

            migrationBuilder.DropColumn(
                name: "Name6",
                table: "DatesSearches");

            migrationBuilder.AlterColumn<int>(
                name: "MenuLE2Id",
                table: "BillReligious",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_BillReligious_MenuLE2_MenuLE2Id",
                table: "BillReligious",
                column: "MenuLE2Id",
                principalTable: "MenuLE2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
