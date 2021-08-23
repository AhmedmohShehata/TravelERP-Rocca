using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class EznAllInOne1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_CustomerOrSuppliers_CustomerOrSupplierId",
                table: "Ezns");

            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_CustomersSuppliers_CustomerSupplierId",
                table: "Ezns");

            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_MenuLE0_MenuLE0Id",
                table: "Ezns");

            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_MenuLZ0_MenuLZ0Id",
                table: "Ezns");

            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_MenuLZ1_MenuLZ1Id",
                table: "Ezns");

            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_MenuLZ2_MenuLZ2Id",
                table: "Ezns");

            migrationBuilder.AlterColumn<int>(
                name: "MenuLZ2Id",
                table: "Ezns",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "MenuLZ1Id",
                table: "Ezns",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "MenuLZ0Id",
                table: "Ezns",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "MenuLE0Id",
                table: "Ezns",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CustomerSupplierId",
                table: "Ezns",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CustomerOrSupplierId",
                table: "Ezns",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_CustomerOrSuppliers_CustomerOrSupplierId",
                table: "Ezns",
                column: "CustomerOrSupplierId",
                principalTable: "CustomerOrSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_CustomersSuppliers_CustomerSupplierId",
                table: "Ezns",
                column: "CustomerSupplierId",
                principalTable: "CustomersSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_MenuLE0_MenuLE0Id",
                table: "Ezns",
                column: "MenuLE0Id",
                principalTable: "MenuLE0",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_MenuLZ0_MenuLZ0Id",
                table: "Ezns",
                column: "MenuLZ0Id",
                principalTable: "MenuLZ0",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_MenuLZ1_MenuLZ1Id",
                table: "Ezns",
                column: "MenuLZ1Id",
                principalTable: "MenuLZ1",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_MenuLZ2_MenuLZ2Id",
                table: "Ezns",
                column: "MenuLZ2Id",
                principalTable: "MenuLZ2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_CustomerOrSuppliers_CustomerOrSupplierId",
                table: "Ezns");

            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_CustomersSuppliers_CustomerSupplierId",
                table: "Ezns");

            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_MenuLE0_MenuLE0Id",
                table: "Ezns");

            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_MenuLZ0_MenuLZ0Id",
                table: "Ezns");

            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_MenuLZ1_MenuLZ1Id",
                table: "Ezns");

            migrationBuilder.DropForeignKey(
                name: "FK_Ezns_MenuLZ2_MenuLZ2Id",
                table: "Ezns");

            migrationBuilder.AlterColumn<int>(
                name: "MenuLZ2Id",
                table: "Ezns",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MenuLZ1Id",
                table: "Ezns",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MenuLZ0Id",
                table: "Ezns",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MenuLE0Id",
                table: "Ezns",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerSupplierId",
                table: "Ezns",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerOrSupplierId",
                table: "Ezns",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_CustomerOrSuppliers_CustomerOrSupplierId",
                table: "Ezns",
                column: "CustomerOrSupplierId",
                principalTable: "CustomerOrSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_CustomersSuppliers_CustomerSupplierId",
                table: "Ezns",
                column: "CustomerSupplierId",
                principalTable: "CustomersSuppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_MenuLE0_MenuLE0Id",
                table: "Ezns",
                column: "MenuLE0Id",
                principalTable: "MenuLE0",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_MenuLZ0_MenuLZ0Id",
                table: "Ezns",
                column: "MenuLZ0Id",
                principalTable: "MenuLZ0",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_MenuLZ1_MenuLZ1Id",
                table: "Ezns",
                column: "MenuLZ1Id",
                principalTable: "MenuLZ1",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ezns_MenuLZ2_MenuLZ2Id",
                table: "Ezns",
                column: "MenuLZ2Id",
                principalTable: "MenuLZ2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
