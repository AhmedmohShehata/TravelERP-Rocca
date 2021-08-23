using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class EditEsal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillReligious_MenuLE2_MenuLE2Id",
                table: "BillReligious");

            migrationBuilder.DropForeignKey(
                name: "FK_Esals_BillAirLines_BillId",
                table: "Esals");

            migrationBuilder.DropIndex(
                name: "IX_Esals_BillId",
                table: "Esals");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillReligious_MenuLE2_MenuLE2Id",
                table: "BillReligious");

            migrationBuilder.AlterColumn<int>(
                name: "MenuLE2Id",
                table: "BillReligious",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Esals_BillId",
                table: "Esals",
                column: "BillId");

            migrationBuilder.AddForeignKey(
                name: "FK_BillReligious_MenuLE2_MenuLE2Id",
                table: "BillReligious",
                column: "MenuLE2Id",
                principalTable: "MenuLE2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Esals_BillAirLines_BillId",
                table: "Esals",
                column: "BillId",
                principalTable: "BillAirLines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
