using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class EditAddItemsToBillDomesticTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GroupsStatus",
                table: "BillDomestic",
                newName: "IndividualStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IndividualStatus",
                table: "BillDomestic",
                newName: "GroupsStatus");
        }
    }
}
