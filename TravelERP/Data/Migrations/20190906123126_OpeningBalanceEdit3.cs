using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class OpeningBalanceEdit3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpeningBalances_StatementTypes_StatementTypeId",
                table: "OpeningBalances");

            migrationBuilder.DropColumn(
                name: "StatementTypeNameId",
                table: "OpeningBalances");

            migrationBuilder.AlterColumn<int>(
                name: "StatementTypeId",
                table: "OpeningBalances",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningBalances_StatementTypes_StatementTypeId",
                table: "OpeningBalances",
                column: "StatementTypeId",
                principalTable: "StatementTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpeningBalances_StatementTypes_StatementTypeId",
                table: "OpeningBalances");

            migrationBuilder.AlterColumn<int>(
                name: "StatementTypeId",
                table: "OpeningBalances",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "StatementTypeNameId",
                table: "OpeningBalances",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningBalances_StatementTypes_StatementTypeId",
                table: "OpeningBalances",
                column: "StatementTypeId",
                principalTable: "StatementTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
