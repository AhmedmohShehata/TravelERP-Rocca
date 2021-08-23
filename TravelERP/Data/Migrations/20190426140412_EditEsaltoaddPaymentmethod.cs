using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class EditEsaltoaddPaymentmethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "paymentMethods",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "Esals",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Esals_PaymentMethodId",
                table: "Esals",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Esals_paymentMethods_PaymentMethodId",
                table: "Esals",
                column: "PaymentMethodId",
                principalTable: "paymentMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Esals_paymentMethods_PaymentMethodId",
                table: "Esals");

            migrationBuilder.DropIndex(
                name: "IX_Esals_PaymentMethodId",
                table: "Esals");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "Esals");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "paymentMethods",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
