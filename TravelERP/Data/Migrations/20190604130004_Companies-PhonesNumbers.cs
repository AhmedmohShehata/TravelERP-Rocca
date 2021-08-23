using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class CompaniesPhonesNumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Company_Address",
                table: "Companies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Company_NameE",
                table: "Companies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Company_PhonesNumber",
                table: "Companies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company_Address",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Company_NameE",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Company_PhonesNumber",
                table: "Companies");
        }
    }
}
