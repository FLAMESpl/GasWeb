using Microsoft.EntityFrameworkCore.Migrations;

namespace GasWeb.Domain.Migrations
{
    public partial class AddConstraintForSubmissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PriceSubmissions_GasStationId",
                table: "PriceSubmissions");

            migrationBuilder.CreateIndex(
                name: "IX_PriceSubmissions_GasStationId_CreatedByUserId_SubmissionDat~",
                table: "PriceSubmissions",
                columns: new[] { "GasStationId", "CreatedByUserId", "SubmissionDate", "FuelType" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PriceSubmissions_GasStationId_CreatedByUserId_SubmissionDat~",
                table: "PriceSubmissions");

            migrationBuilder.CreateIndex(
                name: "IX_PriceSubmissions_GasStationId",
                table: "PriceSubmissions",
                column: "GasStationId");
        }
    }
}
