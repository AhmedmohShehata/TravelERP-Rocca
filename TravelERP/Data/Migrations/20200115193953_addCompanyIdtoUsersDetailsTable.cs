using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class addCompanyIdtoUsersDetailsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyID",
                table: "UsersDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UsersDetails_CompanyID",
                table: "UsersDetails",
                column: "CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersDetails_Companies_CompanyID",
                table: "UsersDetails",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersDetails_Companies_CompanyID",
                table: "UsersDetails");

            migrationBuilder.DropIndex(
                name: "IX_UsersDetails_CompanyID",
                table: "UsersDetails");

            migrationBuilder.DropColumn(
                name: "CompanyID",
                table: "UsersDetails");
        }
    }
}
