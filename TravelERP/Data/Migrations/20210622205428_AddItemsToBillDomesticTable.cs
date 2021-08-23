using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class AddItemsToBillDomesticTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ACustomerPrice",
                table: "BillDomestic",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ANetPrice",
                table: "BillDomestic",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Accommodation",
                table: "BillDomestic",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DTMCustomerPrice",
                table: "BillDomestic",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DTMNetPrice",
                table: "BillDomestic",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DomesticTransportMethod",
                table: "BillDomestic",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ECustomerPrice",
                table: "BillDomestic",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ENetPrice",
                table: "BillDomestic",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Excursion",
                table: "BillDomestic",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GroupsStatus",
                table: "BillDomestic",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TMCustomerPrice",
                table: "BillDomestic",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TMNetPrice",
                table: "BillDomestic",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransportMethodId",
                table: "BillDomestic",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BillDomestic_TransportMethodId",
                table: "BillDomestic",
                column: "TransportMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_BillDomestic_TransportMethods_TransportMethodId",
                table: "BillDomestic",
                column: "TransportMethodId",
                principalTable: "TransportMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillDomestic_TransportMethods_TransportMethodId",
                table: "BillDomestic");

            migrationBuilder.DropIndex(
                name: "IX_BillDomestic_TransportMethodId",
                table: "BillDomestic");

            migrationBuilder.DropColumn(
                name: "ACustomerPrice",
                table: "BillDomestic");

            migrationBuilder.DropColumn(
                name: "ANetPrice",
                table: "BillDomestic");

            migrationBuilder.DropColumn(
                name: "Accommodation",
                table: "BillDomestic");

            migrationBuilder.DropColumn(
                name: "DTMCustomerPrice",
                table: "BillDomestic");

            migrationBuilder.DropColumn(
                name: "DTMNetPrice",
                table: "BillDomestic");

            migrationBuilder.DropColumn(
                name: "DomesticTransportMethod",
                table: "BillDomestic");

            migrationBuilder.DropColumn(
                name: "ECustomerPrice",
                table: "BillDomestic");

            migrationBuilder.DropColumn(
                name: "ENetPrice",
                table: "BillDomestic");

            migrationBuilder.DropColumn(
                name: "Excursion",
                table: "BillDomestic");

            migrationBuilder.DropColumn(
                name: "GroupsStatus",
                table: "BillDomestic");

            migrationBuilder.DropColumn(
                name: "TMCustomerPrice",
                table: "BillDomestic");

            migrationBuilder.DropColumn(
                name: "TMNetPrice",
                table: "BillDomestic");

            migrationBuilder.DropColumn(
                name: "TransportMethodId",
                table: "BillDomestic");
        }
    }
}
