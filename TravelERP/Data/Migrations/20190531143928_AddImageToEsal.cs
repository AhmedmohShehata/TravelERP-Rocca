using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class AddImageToEsal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyA4EsalImage",
                table: "Companies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyLogo",
                table: "Companies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyA4EsalImage",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CompanyLogo",
                table: "Companies");
        }
    }
}
