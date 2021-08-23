using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class MEnuLZ2CompanyId2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuLZ2_Companies_CompanyID",
                table: "MenuLZ2");

            migrationBuilder.RenameColumn(
                name: "CompanyID",
                table: "MenuLZ2",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_MenuLZ2_CompanyID",
                table: "MenuLZ2",
                newName: "IX_MenuLZ2_CompanyId");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "MenuLZ2",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_MenuLZ2_Companies_CompanyId",
                table: "MenuLZ2",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuLZ2_Companies_CompanyId",
                table: "MenuLZ2");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "MenuLZ2",
                newName: "CompanyID");

            migrationBuilder.RenameIndex(
                name: "IX_MenuLZ2_CompanyId",
                table: "MenuLZ2",
                newName: "IX_MenuLZ2_CompanyID");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyID",
                table: "MenuLZ2",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuLZ2_Companies_CompanyID",
                table: "MenuLZ2",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
