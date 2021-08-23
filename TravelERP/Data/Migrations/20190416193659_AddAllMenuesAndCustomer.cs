using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TravelERP.Data.Migrations
{
    public partial class AddAllMenuesAndCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateTable(
                name: "CustomerOrSuppliers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOrSuppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuLE0",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    M0_Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuLE0", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MenuLZ0",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    M0_Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuLZ0", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomersSuppliers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Adrress = table.Column<string>(nullable: true),
                    CustomerOrSupplierId = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNumber1 = table.Column<string>(nullable: false),
                    PhoneNumber2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomersSuppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomersSuppliers_CustomerOrSuppliers_CustomerOrSupplierId",
                        column: x => x.CustomerOrSupplierId,
                        principalTable: "CustomerOrSuppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuLE1",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    M1_Name = table.Column<string>(maxLength: 50, nullable: false),
                    MenuLE0Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuLE1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuLE1_MenuLE0_MenuLE0Id",
                        column: x => x.MenuLE0Id,
                        principalTable: "MenuLE0",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MenuLZ1",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    M1_Name = table.Column<string>(maxLength: 50, nullable: false),
                    MenuLZ0Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuLZ1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuLZ1_MenuLZ0_MenuLZ0Id",
                        column: x => x.MenuLZ0Id,
                        principalTable: "MenuLZ0",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MenuLE2",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    M2_Name = table.Column<string>(maxLength: 50, nullable: false),
                    MenuLE0Id = table.Column<int>(nullable: false),
                    MenuLE1Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuLE2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuLE2_MenuLE0_MenuLE0Id",
                        column: x => x.MenuLE0Id,
                        principalTable: "MenuLE0",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MenuLE2_MenuLE1_MenuLE1Id",
                        column: x => x.MenuLE1Id,
                        principalTable: "MenuLE1",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MenuLZ2",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    M2_Name = table.Column<string>(maxLength: 50, nullable: false),
                    MenuLZ0Id = table.Column<int>(nullable: false),
                    MenuLZ1Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuLZ2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuLZ2_MenuLZ0_MenuLZ0Id",
                        column: x => x.MenuLZ0Id,
                        principalTable: "MenuLZ0",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MenuLZ2_MenuLZ1_MenuLZ1Id",
                        column: x => x.MenuLZ1Id,
                        principalTable: "MenuLZ1",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CustomersSuppliers_CustomerOrSupplierId",
                table: "CustomersSuppliers",
                column: "CustomerOrSupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuLE1_MenuLE0Id",
                table: "MenuLE1",
                column: "MenuLE0Id");

            migrationBuilder.CreateIndex(
                name: "IX_MenuLE2_MenuLE0Id",
                table: "MenuLE2",
                column: "MenuLE0Id");

            migrationBuilder.CreateIndex(
                name: "IX_MenuLE2_MenuLE1Id",
                table: "MenuLE2",
                column: "MenuLE1Id");

            migrationBuilder.CreateIndex(
                name: "IX_MenuLZ1_MenuLZ0Id",
                table: "MenuLZ1",
                column: "MenuLZ0Id");

            migrationBuilder.CreateIndex(
                name: "IX_MenuLZ2_MenuLZ0Id",
                table: "MenuLZ2",
                column: "MenuLZ0Id");

            migrationBuilder.CreateIndex(
                name: "IX_MenuLZ2_MenuLZ1Id",
                table: "MenuLZ2",
                column: "MenuLZ1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CustomersSuppliers");

            migrationBuilder.DropTable(
                name: "MenuLE2");

            migrationBuilder.DropTable(
                name: "MenuLZ2");

            migrationBuilder.DropTable(
                name: "CustomerOrSuppliers");

            migrationBuilder.DropTable(
                name: "MenuLE1");

            migrationBuilder.DropTable(
                name: "MenuLZ1");

            migrationBuilder.DropTable(
                name: "MenuLE0");

            migrationBuilder.DropTable(
                name: "MenuLZ0");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");
        }
    }
}
