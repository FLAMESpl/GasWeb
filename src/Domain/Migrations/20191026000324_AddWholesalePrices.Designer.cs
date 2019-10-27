﻿// <auto-generated />
using System;
using GasWeb.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GasWeb.Domain.Migrations
{
    [DbContext(typeof(GasWebDbContext))]
    [Migration("20191026000324_AddWholesalePrices")]
    partial class AddWholesalePrices
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("GasWeb.Domain.Franchises.Entities.Franchise", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CreatedByUserId");

                    b.Property<DateTime>("LastModified");

                    b.Property<bool>("ManagedBySystem");

                    b.Property<long>("ModifiedByUserId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("ModifiedByUserId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Franchises");
                });

            modelBuilder.Entity("GasWeb.Domain.Franchises.Entities.FranchiseWholesalePrice", b =>
                {
                    b.Property<long>("FranchiseId");

                    b.Property<int>("FuelType");

                    b.Property<decimal>("Amount");

                    b.Property<DateTime>("ModifiedAt");

                    b.HasKey("FranchiseId", "FuelType");

                    b.ToTable("FranchiseWholesalePrice");
                });

            modelBuilder.Entity("GasWeb.Domain.GasStations.Entities.GasStation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CreatedByUserId");

                    b.Property<long?>("FranchiseId");

                    b.Property<DateTime>("LastModified");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<bool>("MaintainedBySystem");

                    b.Property<long>("ModifiedByUserId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("FranchiseId");

                    b.HasIndex("ModifiedByUserId");

                    b.ToTable("GasStations");
                });

            modelBuilder.Entity("GasWeb.Domain.PriceSubmissions.Entities.PriceSubmission", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Amount");

                    b.Property<long>("CreatedByUserId");

                    b.Property<int>("FuelType");

                    b.Property<long>("GasStationId");

                    b.Property<DateTime>("LastModified");

                    b.Property<long>("ModifiedByUserId");

                    b.Property<DateTime>("SubmissionDate")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByUserId");

                    b.HasIndex("GasStationId");

                    b.HasIndex("ModifiedByUserId");

                    b.ToTable("PriceSubmissions");
                });

            modelBuilder.Entity("GasWeb.Domain.Users.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<int>("Role");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GasWeb.Domain.Franchises.Entities.Franchise", b =>
                {
                    b.HasOne("GasWeb.Domain.Users.Entities.User")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GasWeb.Domain.Users.Entities.User")
                        .WithMany()
                        .HasForeignKey("ModifiedByUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GasWeb.Domain.Franchises.Entities.FranchiseWholesalePrice", b =>
                {
                    b.HasOne("GasWeb.Domain.Franchises.Entities.Franchise")
                        .WithMany("WholesalePrices")
                        .HasForeignKey("FranchiseId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GasWeb.Domain.GasStations.Entities.GasStation", b =>
                {
                    b.HasOne("GasWeb.Domain.Users.Entities.User")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GasWeb.Domain.Franchises.Entities.Franchise")
                        .WithMany()
                        .HasForeignKey("FranchiseId");

                    b.HasOne("GasWeb.Domain.Users.Entities.User")
                        .WithMany()
                        .HasForeignKey("ModifiedByUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GasWeb.Domain.PriceSubmissions.Entities.PriceSubmission", b =>
                {
                    b.HasOne("GasWeb.Domain.Users.Entities.User")
                        .WithMany()
                        .HasForeignKey("CreatedByUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GasWeb.Domain.GasStations.Entities.GasStation")
                        .WithMany()
                        .HasForeignKey("GasStationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GasWeb.Domain.Users.Entities.User")
                        .WithMany()
                        .HasForeignKey("ModifiedByUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}