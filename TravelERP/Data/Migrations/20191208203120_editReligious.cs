using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class editReligious : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillReligious_CustomersSuppliers_TicketExportId",
                table: "BillReligious");

            migrationBuilder.DropIndex(
                name: "IX_BillReligious_TicketExportId",
                table: "BillReligious");

            migrationBuilder.DropColumn(
                name: "AdultN",
                table: "BillReligious");

            migrationBuilder.DropColumn(
                name: "ChildN",
                table: "BillReligious");

            migrationBuilder.DropColumn(
                name: "Direction",
                table: "BillReligious");

            migrationBuilder.DropColumn(
                name: "NetPrice",
                table: "BillReligious");

            migrationBuilder.DropColumn(
                name: "PNR",
                table: "BillReligious");

            migrationBuilder.DropColumn(
                name: "TicketExportId",
                table: "BillReligious");

            migrationBuilder.DropColumn(
                name: "TicketFrom",
                table: "BillReligious");

            migrationBuilder.DropColumn(
                name: "TicketTo",
                table: "BillReligious");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdultN",
                table: "BillReligious",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChildN",
                table: "BillReligious",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Direction",
                table: "BillReligious",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NetPrice",
                table: "BillReligious",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PNR",
                table: "BillReligious",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TicketExportId",
                table: "BillReligious",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TicketFrom",
                table: "BillReligious",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TicketTo",
                table: "BillReligious",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BillReligious_TicketExportId",
                table: "BillReligious",
                column: "TicketExportId");

            migrationBuilder.AddForeignKey(
                name: "FK_BillReligious_CustomersSuppliers_TicketExportId",
                table: "BillReligious",
                column: "TicketExportId",
                principalTable: "CustomersSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
