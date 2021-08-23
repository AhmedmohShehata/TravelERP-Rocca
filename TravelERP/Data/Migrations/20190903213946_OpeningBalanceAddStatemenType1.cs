using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class OpeningBalanceAddStatemenType1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StatementType",
                table: "OpeningBalances",
                newName: "StatementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OpeningBalances_StatementTypeId",
                table: "OpeningBalances",
                column: "StatementTypeId");

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

            migrationBuilder.DropIndex(
                name: "IX_OpeningBalances_StatementTypeId",
                table: "OpeningBalances");

            migrationBuilder.RenameColumn(
                name: "StatementTypeId",
                table: "OpeningBalances",
                newName: "StatementType");
        }
    }
}
