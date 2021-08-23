using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class AddCustomerNametoEsal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyID",
                table: "Esals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerOrSupplierId",
                table: "Esals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerSupplierId",
                table: "Esals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TicketExportId",
                table: "Esals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Esals",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Esals_CompanyID",
                table: "Esals",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_Esals_CustomerOrSupplierId",
                table: "Esals",
                column: "CustomerOrSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Esals_CustomerSupplierId",
                table: "Esals",
                column: "CustomerSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Esals_TicketExportId",
                table: "Esals",
                column: "TicketExportId");

            migrationBuilder.CreateIndex(
                name: "IX_Esals_UserId",
                table: "Esals",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Esals_Companies_CompanyID",
                table: "Esals",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Esals_CustomerOrSuppliers_CustomerOrSupplierId",
                table: "Esals",
                column: "CustomerOrSupplierId",
                principalTable: "CustomerOrSuppliers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Esals_CustomersSuppliers_CustomerSupplierId",
                table: "Esals",
                column: "CustomerSupplierId",
                principalTable: "CustomersSuppliers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Esals_CustomersSuppliers_TicketExportId",
                table: "Esals",
                column: "TicketExportId",
                principalTable: "CustomersSuppliers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Esals_AspNetUsers_UserId",
                table: "Esals",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Esals_Companies_CompanyID",
                table: "Esals");

            migrationBuilder.DropForeignKey(
                name: "FK_Esals_CustomerOrSuppliers_CustomerOrSupplierId",
                table: "Esals");

            migrationBuilder.DropForeignKey(
                name: "FK_Esals_CustomersSuppliers_CustomerSupplierId",
                table: "Esals");

            migrationBuilder.DropForeignKey(
                name: "FK_Esals_CustomersSuppliers_TicketExportId",
                table: "Esals");

            migrationBuilder.DropForeignKey(
                name: "FK_Esals_AspNetUsers_UserId",
                table: "Esals");

            migrationBuilder.DropIndex(
                name: "IX_Esals_CompanyID",
                table: "Esals");

            migrationBuilder.DropIndex(
                name: "IX_Esals_CustomerOrSupplierId",
                table: "Esals");

            migrationBuilder.DropIndex(
                name: "IX_Esals_CustomerSupplierId",
                table: "Esals");

            migrationBuilder.DropIndex(
                name: "IX_Esals_TicketExportId",
                table: "Esals");

            migrationBuilder.DropIndex(
                name: "IX_Esals_UserId",
                table: "Esals");

            migrationBuilder.DropColumn(
                name: "CompanyID",
                table: "Esals");

            migrationBuilder.DropColumn(
                name: "CustomerOrSupplierId",
                table: "Esals");

            migrationBuilder.DropColumn(
                name: "CustomerSupplierId",
                table: "Esals");

            migrationBuilder.DropColumn(
                name: "TicketExportId",
                table: "Esals");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Esals");
        }
    }
}
