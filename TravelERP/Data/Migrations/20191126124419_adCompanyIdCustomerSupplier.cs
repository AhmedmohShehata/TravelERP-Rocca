using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class adCompanyIdCustomerSupplier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "CustomersSuppliers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CustomersSuppliers_CompanyId",
                table: "CustomersSuppliers",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomersSuppliers_Companies_CompanyId",
                table: "CustomersSuppliers",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomersSuppliers_Companies_CompanyId",
                table: "CustomersSuppliers");

            migrationBuilder.DropIndex(
                name: "IX_CustomersSuppliers_CompanyId",
                table: "CustomersSuppliers");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "CustomersSuppliers");
        }
    }
}
