using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class eznchangemenu0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_MenuLE0_MenuLE1Id",
                table: "Ezns");

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_MenuLE1_MenuLE1Id",
                table: "Ezns",
                column: "MenuLE1Id",
                principalTable: "MenuLE1",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_MenuLE1_MenuLE1Id",
                table: "Ezns");

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_MenuLE0_MenuLE1Id",
                table: "Ezns",
                column: "MenuLE1Id",
                principalTable: "MenuLE0",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
