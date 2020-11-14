﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjFinalCinelAir.CommonCore.Data;

namespace ProjFinalCinelAir.CommonCore.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
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
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.AwardTicket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Miles")
                        .HasColumnType("int");

                    b.Property<int?>("Seats")
                        .HasColumnType("int");

                    b.Property<int?>("availableSeats")
                        .HasColumnType("int");

                    b.Property<bool>("isValidated")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("AwardTicket");
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.BuyMilesShop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MileQuantity")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("BuyMilesShop");
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Card");
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("City");
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AnnualMilesBought")
                        .HasColumnType("int");

                    b.Property<int>("AnnualMilesConverted")
                        .HasColumnType("int");

                    b.Property<int>("AnnualMilesExtended")
                        .HasColumnType("int");

                    b.Property<int>("AnnualMilesTransfered")
                        .HasColumnType("int");

                    b.Property<int>("Client_Number")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateofBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Identification")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("JoinDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Miles_Bonus")
                        .HasColumnType("int");

                    b.Property<int>("Miles_Status")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TaxNumber")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("isClientNumberConfirmed")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Country");
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.Historic_Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("End_Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Start_Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("StatusId")
                        .HasColumnType("int");

                    b.Property<bool>("isValidated")
                        .HasColumnType("bit");

                    b.Property<bool>("wasNominated")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("StatusId");

                    b.ToTable("Historic_Status");
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.Mile_Bonus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("Miles_Number")
                        .HasColumnType("int");

                    b.Property<DateTime>("Validity")
                        .HasColumnType("datetime2");

                    b.Property<int>("available_Miles_Bonus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Mile_Bonus");
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.Mile_Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("Miles_Number")
                        .HasColumnType("int");

                    b.Property<DateTime>("Validity")
                        .HasColumnType("datetime2");

                    b.Property<int>("available_Miles_Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Mile_Status");
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isRepliedByEmployee")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.Partner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Contact")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isCharity")
                        .HasColumnType("bit");

                    b.Property<bool>("isValidated")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Partner");
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.Rate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Percentage")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Rate");
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Description")
                        .IsUnique()
                        .HasFilter("[Description] IS NOT NULL");

                    b.ToTable("Status");
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Balance_Miles_Bonus")
                        .HasColumnType("int");

                    b.Property<int>("Balance_Miles_Status")
                        .HasColumnType("int");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Miles")
                        .HasColumnType("int");

                    b.Property<string>("Movement_Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.Travel_Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ArrivalCity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<string>("DepartureCity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Miles_BonusId")
                        .HasColumnType("int");

                    b.Property<int?>("Miles_StatusId")
                        .HasColumnType("int");

                    b.Property<int>("RateId")
                        .HasColumnType("int");

                    b.Property<int>("TicketId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Travel_Date")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("Miles_BonusId");

                    b.HasIndex("Miles_StatusId");

                    b.HasIndex("RateId");

                    b.ToTable("Travel_Ticket");
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateofBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Identification")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TaxNumber")
                        .HasColumnType("int");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("isActive")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("Identification")
                        .IsUnique()
                        .HasFilter("[Identification] IS NOT NULL");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("TaxNumber")
                        .IsUnique();

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ProjFinalCinelAir.CommonCore.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ProjFinalCinelAir.CommonCore.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjFinalCinelAir.CommonCore.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ProjFinalCinelAir.CommonCore.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.City", b =>
                {
                    b.HasOne("ProjFinalCinelAir.CommonCore.Data.Entities.Country", null)
                        .WithMany("Cities")
                        .HasForeignKey("CountryId");
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.Client", b =>
                {
                    b.HasOne("ProjFinalCinelAir.CommonCore.Data.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.Historic_Status", b =>
                {
                    b.HasOne("ProjFinalCinelAir.CommonCore.Data.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ProjFinalCinelAir.CommonCore.Data.Entities.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.Mile_Bonus", b =>
                {
                    b.HasOne("ProjFinalCinelAir.CommonCore.Data.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.Mile_Status", b =>
                {
                    b.HasOne("ProjFinalCinelAir.CommonCore.Data.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.Notification", b =>
                {
                    b.HasOne("ProjFinalCinelAir.CommonCore.Data.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.Transaction", b =>
                {
                    b.HasOne("ProjFinalCinelAir.CommonCore.Data.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.Travel_Ticket", b =>
                {
                    b.HasOne("ProjFinalCinelAir.CommonCore.Data.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ProjFinalCinelAir.CommonCore.Data.Entities.Mile_Bonus", "Miles_Bonus")
                        .WithMany()
                        .HasForeignKey("Miles_BonusId");

                    b.HasOne("ProjFinalCinelAir.CommonCore.Data.Entities.Mile_Status", "Miles_Status")
                        .WithMany()
                        .HasForeignKey("Miles_StatusId");

                    b.HasOne("ProjFinalCinelAir.CommonCore.Data.Entities.Rate", "Rate")
                        .WithMany()
                        .HasForeignKey("RateId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjFinalCinelAir.CommonCore.Data.Entities.User", b =>
                {
                    b.HasOne("ProjFinalCinelAir.CommonCore.Data.Entities.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
