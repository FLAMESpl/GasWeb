using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GasWeb.Domain.Migrations
{
    public partial class AddPriceSubmissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceSubmissions");
        }
    }
}
