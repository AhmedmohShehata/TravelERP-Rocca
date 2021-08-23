using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class EditBillForeignName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_billForeigns_Companies_CompanyID",
                table: "billForeigns");

            migrationBuilder.DropForeignKey(
                name: "FK_billForeigns_CustomerOrSuppliers_CustomerOrSupplierId",
                table: "billForeigns");

            migrationBuilder.DropForeignKey(
                name: "FK_billForeigns_CustomersSuppliers_CustomerSupplierId",
                table: "billForeigns");

            migrationBuilder.DropForeignKey(
                name: "FK_billForeigns_MenuLE0_MenuLE0Id",
                table: "billForeigns");

            migrationBuilder.DropForeignKey(
                name: "FK_billForeigns_MenuLE1_MenuLE1Id",
                table: "billForeigns");

            migrationBuilder.DropForeignKey(
                name: "FK_billForeigns_MenuLE2_MenuLE2Id",
                table: "billForeigns");

            migrationBuilder.DropForeignKey(
                name: "FK_billForeigns_CustomersSuppliers_TicketExportId",
                table: "billForeigns");

            migrationBuilder.DropForeignKey(
                name: "FK_billForeigns_AspNetUsers_UserId",
                table: "billForeigns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_billForeigns",
                table: "billForeigns");

            migrationBuilder.RenameTable(
                name: "billForeigns",
                newName: "BillForeigns");

            migrationBuilder.RenameIndex(
                name: "IX_billForeigns_UserId",
                table: "BillForeigns",
                newName: "IX_BillForeigns_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_billForeigns_TicketExportId",
                table: "BillForeigns",
                newName: "IX_BillForeigns_TicketExportId");

            migrationBuilder.RenameIndex(
                name: "IX_billForeigns_MenuLE2Id",
                table: "BillForeigns",
                newName: "IX_BillForeigns_MenuLE2Id");

            migrationBuilder.RenameIndex(
                name: "IX_billForeigns_MenuLE1Id",
                table: "BillForeigns",
                newName: "IX_BillForeigns_MenuLE1Id");

            migrationBuilder.RenameIndex(
                name: "IX_billForeigns_MenuLE0Id",
                table: "BillForeigns",
                newName: "IX_BillForeigns_MenuLE0Id");

            migrationBuilder.RenameIndex(
                name: "IX_billForeigns_CustomerSupplierId",
                table: "BillForeigns",
                newName: "IX_BillForeigns_CustomerSupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_billForeigns_CustomerOrSupplierId",
                table: "BillForeigns",
                newName: "IX_BillForeigns_CustomerOrSupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_billForeigns_CompanyID",
                table: "BillForeigns",
                newName: "IX_BillForeigns_CompanyID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BillForeigns",
                table: "BillForeigns",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BillForeigns_Companies_CompanyID",
                table: "BillForeigns",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_BillForeigns_CustomerOrSuppliers_CustomerOrSupplierId",
                table: "BillForeigns",
                column: "CustomerOrSupplierId",
                principalTable: "CustomerOrSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_BillForeigns_CustomersSuppliers_CustomerSupplierId",
                table: "BillForeigns",
                column: "CustomerSupplierId",
                principalTable: "CustomersSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_BillForeigns_MenuLE0_MenuLE0Id",
                table: "BillForeigns",
                column: "MenuLE0Id",
                principalTable: "MenuLE0",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_BillForeigns_MenuLE1_MenuLE1Id",
                table: "BillForeigns",
                column: "MenuLE1Id",
                principalTable: "MenuLE1",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_BillForeigns_MenuLE2_MenuLE2Id",
                table: "BillForeigns",
                column: "MenuLE2Id",
                principalTable: "MenuLE2",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_BillForeigns_CustomersSuppliers_TicketExportId",
                table: "BillForeigns",
                column: "TicketExportId",
                principalTable: "CustomersSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_BillForeigns_AspNetUsers_UserId",
                table: "BillForeigns",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillForeigns_Companies_CompanyID",
                table: "BillForeigns");

            migrationBuilder.DropForeignKey(
                name: "FK_BillForeigns_CustomerOrSuppliers_CustomerOrSupplierId",
                table: "BillForeigns");

            migrationBuilder.DropForeignKey(
                name: "FK_BillForeigns_CustomersSuppliers_CustomerSupplierId",
                table: "BillForeigns");

            migrationBuilder.DropForeignKey(
                name: "FK_BillForeigns_MenuLE0_MenuLE0Id",
                table: "BillForeigns");

            migrationBuilder.DropForeignKey(
                name: "FK_BillForeigns_MenuLE1_MenuLE1Id",
                table: "BillForeigns");

            migrationBuilder.DropForeignKey(
                name: "FK_BillForeigns_MenuLE2_MenuLE2Id",
                table: "BillForeigns");

            migrationBuilder.DropForeignKey(
                name: "FK_BillForeigns_CustomersSuppliers_TicketExportId",
                table: "BillForeigns");

            migrationBuilder.DropForeignKey(
                name: "FK_BillForeigns_AspNetUsers_UserId",
                table: "BillForeigns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BillForeigns",
                table: "BillForeigns");

            migrationBuilder.RenameTable(
                name: "BillForeigns",
                newName: "billForeigns");

            migrationBuilder.RenameIndex(
                name: "IX_BillForeigns_UserId",
                table: "billForeigns",
                newName: "IX_billForeigns_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BillForeigns_TicketExportId",
                table: "billForeigns",
                newName: "IX_billForeigns_TicketExportId");

            migrationBuilder.RenameIndex(
                name: "IX_BillForeigns_MenuLE2Id",
                table: "billForeigns",
                newName: "IX_billForeigns_MenuLE2Id");

            migrationBuilder.RenameIndex(
                name: "IX_BillForeigns_MenuLE1Id",
                table: "billForeigns",
                newName: "IX_billForeigns_MenuLE1Id");

            migrationBuilder.RenameIndex(
                name: "IX_BillForeigns_MenuLE0Id",
                table: "billForeigns",
                newName: "IX_billForeigns_MenuLE0Id");

            migrationBuilder.RenameIndex(
                name: "IX_BillForeigns_CustomerSupplierId",
                table: "billForeigns",
                newName: "IX_billForeigns_CustomerSupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_BillForeigns_CustomerOrSupplierId",
                table: "billForeigns",
                newName: "IX_billForeigns_CustomerOrSupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_BillForeigns_CompanyID",
                table: "billForeigns",
                newName: "IX_billForeigns_CompanyID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_billForeigns",
                table: "billForeigns",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_billForeigns_Companies_CompanyID",
                table: "billForeigns",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_billForeigns_CustomerOrSuppliers_CustomerOrSupplierId",
                table: "billForeigns",
                column: "CustomerOrSupplierId",
                principalTable: "CustomerOrSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_billForeigns_CustomersSuppliers_CustomerSupplierId",
                table: "billForeigns",
                column: "CustomerSupplierId",
                principalTable: "CustomersSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_billForeigns_MenuLE0_MenuLE0Id",
                table: "billForeigns",
                column: "MenuLE0Id",
                principalTable: "MenuLE0",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_billForeigns_MenuLE1_MenuLE1Id",
                table: "billForeigns",
                column: "MenuLE1Id",
                principalTable: "MenuLE1",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_billForeigns_MenuLE2_MenuLE2Id",
                table: "billForeigns",
                column: "MenuLE2Id",
                principalTable: "MenuLE2",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_billForeigns_CustomersSuppliers_TicketExportId",
                table: "billForeigns",
                column: "TicketExportId",
                principalTable: "CustomersSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_billForeigns_AspNetUsers_UserId",
                table: "billForeigns",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
