using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class EznsForEsalsAddCSCOS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerOrSupplierId",
                table: "EznsForEsals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerSupplierId",
                table: "EznsForEsals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EznsForEsals_CustomerOrSupplierId",
                table: "EznsForEsals",
                column: "CustomerOrSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_EznsForEsals_CustomerSupplierId",
                table: "EznsForEsals",
                column: "CustomerSupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_EznsForEsals_CustomerOrSuppliers_CustomerOrSupplierId",
                table: "EznsForEsals",
                column: "CustomerOrSupplierId",
                principalTable: "CustomerOrSuppliers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EznsForEsals_CustomersSuppliers_CustomerSupplierId",
                table: "EznsForEsals",
                column: "CustomerSupplierId",
                principalTable: "CustomersSuppliers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EznsForEsals_CustomerOrSuppliers_CustomerOrSupplierId",
                table: "EznsForEsals");

            migrationBuilder.DropForeignKey(
                name: "FK_EznsForEsals_CustomersSuppliers_CustomerSupplierId",
                table: "EznsForEsals");

            migrationBuilder.DropIndex(
                name: "IX_EznsForEsals_CustomerOrSupplierId",
                table: "EznsForEsals");

            migrationBuilder.DropIndex(
                name: "IX_EznsForEsals_CustomerSupplierId",
                table: "EznsForEsals");

            migrationBuilder.DropColumn(
                name: "CustomerOrSupplierId",
                table: "EznsForEsals");

            migrationBuilder.DropColumn(
                name: "CustomerSupplierId",
                table: "EznsForEsals");
        }
    }
}
