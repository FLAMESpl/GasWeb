using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GasWeb.Domain.Migrations
{
    public partial class AddWholesalePrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FranchiseWholesalePrice",
                columns: table => new
                {
                    FranchiseId = table.Column<long>(nullable: false),
                    FuelType = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FranchiseWholesalePrice", x => new { x.FranchiseId, x.FuelType });
                    table.ForeignKey(
                        name: "FK_FranchiseWholesalePrice_Franchises_FranchiseId",
                        column: x => x.FranchiseId,
                        principalTable: "Franchises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Franchises_Name",
                table: "Franchises",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FranchiseWholesalePrice");

            migrationBuilder.DropIndex(
                name: "IX_Franchises_Name",
                table: "Franchises");
        }
    }
}
