using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class editTransportMethodTripSeatCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillId",
                table: "TransportMethodTrips");

            migrationBuilder.RenameColumn(
                name: "BillIdId",
                table: "TransportMethodTrips",
                newName: "SeatsCount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SeatsCount",
                table: "TransportMethodTrips",
                newName: "BillIdId");

            migrationBuilder.AddColumn<int>(
                name: "BillId",
                table: "TransportMethodTrips",
                nullable: false,
                defaultValue: 0);
        }
    }
}
