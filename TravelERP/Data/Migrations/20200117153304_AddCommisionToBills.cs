using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class AddCommisionToBills : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "EMPCommission",
                table: "BillVisas",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "EMPCommission",
                table: "BillDomestic",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "EMPCommission",
                table: "BillAirLines",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EMPCommission",
                table: "BillVisas");

            migrationBuilder.DropColumn(
                name: "EMPCommission",
                table: "BillDomestic");

            migrationBuilder.DropColumn(
                name: "EMPCommission",
                table: "BillAirLines");
        }
    }
}
