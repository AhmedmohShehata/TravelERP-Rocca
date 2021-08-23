﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using TravelERP.Data;

namespace TravelERP.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190604130004_Companies-PhonesNumbers")]
    partial class CompaniesPhonesNumbers
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TravelERP.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<int>("CompanyId");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("PhoneNumber1")
                        .IsRequired();

                    b.Property<string>("PhoneNumber2");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TravelERP.Models.BillAirLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AdultN");

                    b.Property<DateTime>("BillDate");

                    b.Property<int>("BillId");

                    b.Property<int>("ChildN");

                    b.Property<string>("Commnets");

                    b.Property<int>("CompanyID");

                    b.Property<int>("CustomerOrSupplierId");

                    b.Property<int>("CustomerPrice");

                    b.Property<int>("CustomerSupplierId");

                    b.Property<string>("Direction");

                    b.Property<int>("MenuLE0Id");

                    b.Property<int>("MenuLE1Id");

                    b.Property<int>("MenuLE2Id");

                    b.Property<int>("NetPrice");

                    b.Property<string>("PNR");

                    b.Property<int>("TicketExportId");

                    b.Property<DateTime>("TicketFrom");

                    b.Property<DateTime?>("TicketTo");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CompanyID");

                    b.HasIndex("CustomerOrSupplierId");

                    b.HasIndex("CustomerSupplierId");

                    b.HasIndex("MenuLE0Id");

                    b.HasIndex("MenuLE1Id");

                    b.HasIndex("MenuLE2Id");

                    b.HasIndex("TicketExportId");

                    b.HasIndex("UserId");

                    b.ToTable("BillAirLines");
                });

            modelBuilder.Entity("TravelERP.Models.BillDomestic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AdultN");

                    b.Property<DateTime>("BillDate");

                    b.Property<int>("BillId");

                    b.Property<int>("ChildN");

                    b.Property<string>("Commnets");

                    b.Property<int>("CompanyID");

                    b.Property<int>("CustomerOrSupplierId");

                    b.Property<int>("CustomerPrice");

                    b.Property<int>("CustomerSupplierId");

                    b.Property<int>("MenuLE0Id");

                    b.Property<int>("MenuLE1Id");

                    b.Property<int>("MenuLE2Id");

                    b.Property<int>("NetPrice");

                    b.Property<int>("TicketExportId");

                    b.Property<DateTime>("TicketFrom");

                    b.Property<DateTime?>("TicketTo");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CompanyID");

                    b.HasIndex("CustomerOrSupplierId");

                    b.HasIndex("CustomerSupplierId");

                    b.HasIndex("MenuLE0Id");

                    b.HasIndex("MenuLE1Id");

                    b.HasIndex("MenuLE2Id");

                    b.HasIndex("TicketExportId");

                    b.HasIndex("UserId");

                    b.ToTable("BillDomestic");
                });

            modelBuilder.Entity("TravelERP.Models.BillReligious", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AdultN");

                    b.Property<DateTime>("BillDate");

                    b.Property<int>("BillId");

                    b.Property<int>("ChildN");

                    b.Property<string>("Commnets");

                    b.Property<int>("CompanyID");

                    b.Property<int>("CustomerOrSupplierId");

                    b.Property<int>("CustomerPrice");

                    b.Property<int>("CustomerSupplierId");

                    b.Property<string>("Direction");

                    b.Property<int>("MenuLE0Id");

                    b.Property<int>("MenuLE1Id");

                    b.Property<int?>("MenuLE2Id");

                    b.Property<int>("NetPrice");

                    b.Property<string>("PNR");

                    b.Property<int>("TicketExportId");

                    b.Property<DateTime>("TicketFrom");

                    b.Property<DateTime?>("TicketTo");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CompanyID");

                    b.HasIndex("CustomerOrSupplierId");

                    b.HasIndex("CustomerSupplierId");

                    b.HasIndex("MenuLE0Id");

                    b.HasIndex("MenuLE1Id");

                    b.HasIndex("MenuLE2Id");

                    b.HasIndex("TicketExportId");

                    b.HasIndex("UserId");

                    b.ToTable("BillReligious");
                });

            modelBuilder.Entity("TravelERP.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyA4EsalImage");

                    b.Property<string>("CompanyLogo");

                    b.Property<string>("Company_Address");

                    b.Property<string>("Company_Name")
                        .IsRequired();

                    b.Property<string>("Company_NameE");

                    b.Property<string>("Company_PhonesNumber");

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("TravelERP.Models.CustomerOrSupplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("CustomerOrSuppliers");
                });

            modelBuilder.Entity("TravelERP.Models.CustomerSupplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Adrress");

                    b.Property<int>("CustomerOrSupplierId");

                    b.Property<string>("Email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("PhoneNumber1")
                        .IsRequired();

                    b.Property<string>("PhoneNumber2");

                    b.HasKey("Id");

                    b.HasIndex("CustomerOrSupplierId");

                    b.ToTable("CustomersSuppliers");
                });

            modelBuilder.Entity("TravelERP.Models.Esal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AmountPaid");

                    b.Property<int?>("BillId")
                        .IsRequired();

                    b.Property<int?>("BillIdId");

                    b.Property<int>("CompanyID");

                    b.Property<int>("CustomerOrSupplierId");

                    b.Property<int>("CustomerSupplierId");

                    b.Property<DateTime>("EsalDate");

                    b.Property<int>("EsalId");

                    b.Property<int>("MenuLE0Id");

                    b.Property<int>("MenuLE1Id");

                    b.Property<int>("MenuLE2Id");

                    b.Property<int>("PaymentMethodId");

                    b.Property<int>("TicketExportId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CompanyID");

                    b.HasIndex("CustomerOrSupplierId");

                    b.HasIndex("CustomerSupplierId");

                    b.HasIndex("MenuLE0Id");

                    b.HasIndex("MenuLE1Id");

                    b.HasIndex("MenuLE2Id");

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("TicketExportId");

                    b.HasIndex("UserId");

                    b.ToTable("Esals");
                });

            modelBuilder.Entity("TravelERP.Models.MenuLE0", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("M0_Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("MenuLE0");
                });

            modelBuilder.Entity("TravelERP.Models.MenuLE1", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("M1_Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("MenuLE0Id");

                    b.HasKey("Id");

                    b.HasIndex("MenuLE0Id");

                    b.ToTable("MenuLE1");
                });

            modelBuilder.Entity("TravelERP.Models.MenuLE2", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("M2_Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("MenuLE0Id");

                    b.Property<int>("MenuLE1Id");

                    b.HasKey("Id");

                    b.HasIndex("MenuLE0Id");

                    b.HasIndex("MenuLE1Id");

                    b.ToTable("MenuLE2");
                });

            modelBuilder.Entity("TravelERP.Models.MenuLZ0", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("M0_Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("MenuLZ0");
                });

            modelBuilder.Entity("TravelERP.Models.MenuLZ1", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("M1_Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("MenuLZ0Id");

                    b.HasKey("Id");

                    b.HasIndex("MenuLZ0Id");

                    b.ToTable("MenuLZ1");
                });

            modelBuilder.Entity("TravelERP.Models.MenuLZ2", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("M2_Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("MenuLZ0Id");

                    b.Property<int>("MenuLZ1Id");

                    b.HasKey("Id");

                    b.HasIndex("MenuLZ0Id");

                    b.HasIndex("MenuLZ1Id");

                    b.ToTable("MenuLZ2");
                });

            modelBuilder.Entity("TravelERP.Models.PaymentMethod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("paymentMethods");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TravelERP.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TravelERP.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TravelERP.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TravelERP.Models.ApplicationUser", b =>
                {
                    b.HasOne("TravelERP.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TravelERP.Models.BillAirLine", b =>
                {
                    b.HasOne("TravelERP.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.CustomerOrSupplier", "CustomerOrSupplier")
                        .WithMany()
                        .HasForeignKey("CustomerOrSupplierId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.CustomerSupplier", "CustomerSupplier")
                        .WithMany()
                        .HasForeignKey("CustomerSupplierId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.MenuLE0", "MenuLE0")
                        .WithMany()
                        .HasForeignKey("MenuLE0Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.MenuLE1", "MenuLE1")
                        .WithMany()
                        .HasForeignKey("MenuLE1Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.MenuLE2", "MenuLE2")
                        .WithMany()
                        .HasForeignKey("MenuLE2Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.CustomerSupplier", "TicketExport")
                        .WithMany()
                        .HasForeignKey("TicketExportId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TravelERP.Models.BillDomestic", b =>
                {
                    b.HasOne("TravelERP.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.CustomerOrSupplier", "CustomerOrSupplier")
                        .WithMany()
                        .HasForeignKey("CustomerOrSupplierId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.CustomerSupplier", "CustomerSupplier")
                        .WithMany()
                        .HasForeignKey("CustomerSupplierId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.MenuLE0", "MenuLE0")
                        .WithMany()
                        .HasForeignKey("MenuLE0Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.MenuLE1", "MenuLE1")
                        .WithMany()
                        .HasForeignKey("MenuLE1Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.MenuLE2", "MenuLE2")
                        .WithMany()
                        .HasForeignKey("MenuLE2Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.CustomerSupplier", "TicketExport")
                        .WithMany()
                        .HasForeignKey("TicketExportId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TravelERP.Models.BillReligious", b =>
                {
                    b.HasOne("TravelERP.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.CustomerOrSupplier", "CustomerOrSupplier")
                        .WithMany()
                        .HasForeignKey("CustomerOrSupplierId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.CustomerSupplier", "CustomerSupplier")
                        .WithMany()
                        .HasForeignKey("CustomerSupplierId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.MenuLE0", "MenuLE0")
                        .WithMany()
                        .HasForeignKey("MenuLE0Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.MenuLE1", "MenuLE1")
                        .WithMany()
                        .HasForeignKey("MenuLE1Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.MenuLE2", "MenuLE2")
                        .WithMany()
                        .HasForeignKey("MenuLE2Id");

                    b.HasOne("TravelERP.Models.CustomerSupplier", "TicketExport")
                        .WithMany()
                        .HasForeignKey("TicketExportId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TravelERP.Models.CustomerSupplier", b =>
                {
                    b.HasOne("TravelERP.Models.CustomerOrSupplier", "CustomerOrSupplier")
                        .WithMany()
                        .HasForeignKey("CustomerOrSupplierId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TravelERP.Models.Esal", b =>
                {
                    b.HasOne("TravelERP.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.CustomerOrSupplier", "CustomerOrSupplier")
                        .WithMany()
                        .HasForeignKey("CustomerOrSupplierId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.CustomerSupplier", "CustomerSupplier")
                        .WithMany()
                        .HasForeignKey("CustomerSupplierId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.MenuLE0", "MenuLE0")
                        .WithMany()
                        .HasForeignKey("MenuLE0Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.MenuLE1", "MenuLE1")
                        .WithMany()
                        .HasForeignKey("MenuLE1Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.MenuLE2", "MenuLE2")
                        .WithMany()
                        .HasForeignKey("MenuLE2Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.PaymentMethod", "PaymentMethod")
                        .WithMany()
                        .HasForeignKey("PaymentMethodId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.CustomerSupplier", "TicketExport")
                        .WithMany()
                        .HasForeignKey("TicketExportId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TravelERP.Models.MenuLE1", b =>
                {
                    b.HasOne("TravelERP.Models.MenuLE0", "MenuLE0")
                        .WithMany()
                        .HasForeignKey("MenuLE0Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TravelERP.Models.MenuLE2", b =>
                {
                    b.HasOne("TravelERP.Models.MenuLE0", "MenuLE0")
                        .WithMany()
                        .HasForeignKey("MenuLE0Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.MenuLE1", "MenuLE1")
                        .WithMany()
                        .HasForeignKey("MenuLE1Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TravelERP.Models.MenuLZ1", b =>
                {
                    b.HasOne("TravelERP.Models.MenuLZ0", "MenuLZ0")
                        .WithMany()
                        .HasForeignKey("MenuLZ0Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TravelERP.Models.MenuLZ2", b =>
                {
                    b.HasOne("TravelERP.Models.MenuLZ0", "MenuLZ0")
                        .WithMany()
                        .HasForeignKey("MenuLZ0Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TravelERP.Models.MenuLZ1", "MenuLZ1")
                        .WithMany()
                        .HasForeignKey("MenuLZ1Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
