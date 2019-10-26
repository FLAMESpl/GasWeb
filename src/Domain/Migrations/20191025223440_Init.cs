﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GasWeb.Domain.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Role = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Franchises",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedByUserId = table.Column<long>(nullable: false),
                    ModifiedByUserId = table.Column<long>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ManagedBySystem = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Franchises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Franchises_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Franchises_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GasStations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedByUserId = table.Column<long>(nullable: false),
                    ModifiedByUserId = table.Column<long>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    FranchiseId = table.Column<long>(nullable: true),
                    MaintainedBySystem = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GasStations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GasStations_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GasStations_Franchises_FranchiseId",
                        column: x => x.FranchiseId,
                        principalTable: "Franchises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GasStations_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriceSubmissions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedByUserId = table.Column<long>(nullable: false),
                    ModifiedByUserId = table.Column<long>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: false),
                    GasStationId = table.Column<long>(nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    FuelType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceSubmissions_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceSubmissions_GasStations_GasStationId",
                        column: x => x.GasStationId,
                        principalTable: "GasStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceSubmissions_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Franchises_CreatedByUserId",
                table: "Franchises",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Franchises_ModifiedByUserId",
                table: "Franchises",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GasStations_CreatedByUserId",
                table: "GasStations",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GasStations_FranchiseId",
                table: "GasStations",
                column: "FranchiseId");

            migrationBuilder.CreateIndex(
                name: "IX_GasStations_ModifiedByUserId",
                table: "GasStations",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceSubmissions_CreatedByUserId",
                table: "PriceSubmissions",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceSubmissions_GasStationId",
                table: "PriceSubmissions",
                column: "GasStationId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceSubmissions_ModifiedByUserId",
                table: "PriceSubmissions",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceSubmissions");

            migrationBuilder.DropTable(
                name: "GasStations");

            migrationBuilder.DropTable(
                name: "Franchises");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
