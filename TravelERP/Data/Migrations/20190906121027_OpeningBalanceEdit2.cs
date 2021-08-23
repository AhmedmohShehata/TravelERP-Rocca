using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class OpeningBalanceEdit2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpeningBalances_MenuLE0_MenuLE0Id",
                table: "OpeningBalances");

            migrationBuilder.DropForeignKey(
                name: "FK_OpeningBalances_StatementTypes_StatementTypeId",
                table: "OpeningBalances");

            migrationBuilder.RenameColumn(
                name: "MenuLE0NameId",
                table: "OpeningBalances",
                newName: "StatementTypeNameId");

            migrationBuilder.AlterColumn<int>(
                name: "StatementTypeId",
                table: "OpeningBalances",
                nullable: true,
                oldClrType: typeof(int));

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

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningBalances_StatementTypes_StatementTypeId",
                table: "OpeningBalances",
                column: "StatementTypeId",
                principalTable: "StatementTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpeningBalances_MenuLE0_MenuLE0Id",
                table: "OpeningBalances");

            migrationBuilder.DropForeignKey(
                name: "FK_OpeningBalances_StatementTypes_StatementTypeId",
                table: "OpeningBalances");

            migrationBuilder.RenameColumn(
                name: "StatementTypeNameId",
                table: "OpeningBalances",
                newName: "MenuLE0NameId");

            migrationBuilder.AlterColumn<int>(
                name: "StatementTypeId",
                table: "OpeningBalances",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MenuLE0Id",
                table: "OpeningBalances",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningBalances_MenuLE0_MenuLE0Id",
                table: "OpeningBalances",
                column: "MenuLE0Id",
                principalTable: "MenuLE0",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningBalances_StatementTypes_StatementTypeId",
                table: "OpeningBalances",
                column: "StatementTypeId",
                principalTable: "StatementTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
