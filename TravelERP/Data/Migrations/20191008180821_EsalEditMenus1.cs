using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class EsalEditMenus1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Esals_CustomersSuppliers_TicketExportId",
                table: "Esals");

            migrationBuilder.AlterColumn<int>(
                name: "TicketExportId",
                table: "Esals",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Esals_CustomersSuppliers_TicketExportId",
                table: "Esals",
                column: "TicketExportId",
                principalTable: "CustomersSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Esals_CustomersSuppliers_TicketExportId",
                table: "Esals");

            migrationBuilder.AlterColumn<int>(
                name: "TicketExportId",
                table: "Esals",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Esals_CustomersSuppliers_TicketExportId",
                table: "Esals",
                column: "TicketExportId",
                principalTable: "CustomersSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
