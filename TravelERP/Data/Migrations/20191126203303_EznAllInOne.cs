using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class EznAllInOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerOrSupplierId",
                table: "Ezns",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerSupplierId",
                table: "Ezns",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MenuLE0Id",
                table: "Ezns",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ezns_CustomerOrSupplierId",
                table: "Ezns",
                column: "CustomerOrSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Ezns_CustomerSupplierId",
                table: "Ezns",
                column: "CustomerSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Ezns_MenuLE0Id",
                table: "Ezns",
                column: "MenuLE0Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_CustomerOrSuppliers_CustomerOrSupplierId",
                table: "Ezns",
                column: "CustomerOrSupplierId",
                principalTable: "CustomerOrSuppliers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_CustomersSuppliers_CustomerSupplierId",
                table: "Ezns",
                column: "CustomerSupplierId",
                principalTable: "CustomersSuppliers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_MenuLE0_MenuLE0Id",
                table: "Ezns",
                column: "MenuLE0Id",
                principalTable: "MenuLE0",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_CustomerOrSuppliers_CustomerOrSupplierId",
                table: "Ezns");

            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_CustomersSuppliers_CustomerSupplierId",
                table: "Ezns");

            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_MenuLE0_MenuLE0Id",
                table: "Ezns");

            migrationBuilder.DropIndex(
                name: "IX_Ezns_CustomerOrSupplierId",
                table: "Ezns");

            migrationBuilder.DropIndex(
                name: "IX_Ezns_CustomerSupplierId",
                table: "Ezns");

            migrationBuilder.DropIndex(
                name: "IX_Ezns_MenuLE0Id",
                table: "Ezns");

            migrationBuilder.DropColumn(
                name: "CustomerOrSupplierId",
                table: "Ezns");

            migrationBuilder.DropColumn(
                name: "CustomerSupplierId",
                table: "Ezns");

            migrationBuilder.DropColumn(
                name: "MenuLE0Id",
                table: "Ezns");
        }
    }
}
