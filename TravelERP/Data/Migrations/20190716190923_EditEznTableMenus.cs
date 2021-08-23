using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class EditEznTableMenus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_MenuLZ0_MenuLZ0Id",
                table: "Ezns");

            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_MenuLZ1_MenuLZ1Id",
                table: "Ezns");

            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_MenuLZ2_MenuLZ2Id",
                table: "Ezns");

            migrationBuilder.RenameColumn(
                name: "MenuLZ2Id",
                table: "Ezns",
                newName: "MenuLE2Id");

            migrationBuilder.RenameColumn(
                name: "MenuLZ1Id",
                table: "Ezns",
                newName: "MenuLE1Id");

            migrationBuilder.RenameColumn(
                name: "MenuLZ0Id",
                table: "Ezns",
                newName: "MenuLE0Id");

            migrationBuilder.RenameIndex(
                name: "IX_Ezns_MenuLZ2Id",
                table: "Ezns",
                newName: "IX_Ezns_MenuLE2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Ezns_MenuLZ1Id",
                table: "Ezns",
                newName: "IX_Ezns_MenuLE1Id");

            migrationBuilder.RenameIndex(
                name: "IX_Ezns_MenuLZ0Id",
                table: "Ezns",
                newName: "IX_Ezns_MenuLE0Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_MenuLZ0_MenuLE0Id",
                table: "Ezns",
                column: "MenuLE0Id",
                principalTable: "MenuLZ0",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_MenuLZ1_MenuLE1Id",
                table: "Ezns",
                column: "MenuLE1Id",
                principalTable: "MenuLZ1",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_MenuLZ2_MenuLE2Id",
                table: "Ezns",
                column: "MenuLE2Id",
                principalTable: "MenuLZ2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_MenuLZ0_MenuLE0Id",
                table: "Ezns");

            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_MenuLZ1_MenuLE1Id",
                table: "Ezns");

            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_MenuLZ2_MenuLE2Id",
                table: "Ezns");

            migrationBuilder.RenameColumn(
                name: "MenuLE2Id",
                table: "Ezns",
                newName: "MenuLZ2Id");

            migrationBuilder.RenameColumn(
                name: "MenuLE1Id",
                table: "Ezns",
                newName: "MenuLZ1Id");

            migrationBuilder.RenameColumn(
                name: "MenuLE0Id",
                table: "Ezns",
                newName: "MenuLZ0Id");

            migrationBuilder.RenameIndex(
                name: "IX_Ezns_MenuLE2Id",
                table: "Ezns",
                newName: "IX_Ezns_MenuLZ2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Ezns_MenuLE1Id",
                table: "Ezns",
                newName: "IX_Ezns_MenuLZ1Id");

            migrationBuilder.RenameIndex(
                name: "IX_Ezns_MenuLE0Id",
                table: "Ezns",
                newName: "IX_Ezns_MenuLZ0Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_MenuLZ0_MenuLZ0Id",
                table: "Ezns",
                column: "MenuLZ0Id",
                principalTable: "MenuLZ0",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_MenuLZ1_MenuLZ1Id",
                table: "Ezns",
                column: "MenuLZ1Id",
                principalTable: "MenuLZ1",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_MenuLZ2_MenuLZ2Id",
                table: "Ezns",
                column: "MenuLZ2Id",
                principalTable: "MenuLZ2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
