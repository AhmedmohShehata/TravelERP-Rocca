using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class OpeningBalanceEdit1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpeningBalances_MenuLE0_MenuLE0Id",
                table: "OpeningBalances");

            migrationBuilder.AlterColumn<int>(
                name: "MenuLE0Id",
                table: "OpeningBalances",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "MenuLE0NameId",
                table: "OpeningBalances",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningBalances_MenuLE0_MenuLE0Id",
                table: "OpeningBalances",
                column: "MenuLE0Id",
                principalTable: "MenuLE0",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpeningBalances_MenuLE0_MenuLE0Id",
                table: "OpeningBalances");

            migrationBuilder.DropColumn(
                name: "MenuLE0NameId",
                table: "OpeningBalances");

            migrationBuilder.AlterColumn<int>(
                name: "MenuLE0Id",
                table: "OpeningBalances",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningBalances_MenuLE0_MenuLE0Id",
                table: "OpeningBalances",
                column: "MenuLE0Id",
                principalTable: "MenuLE0",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
