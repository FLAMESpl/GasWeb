using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GasWeb.Domain.Migrations
{
    public partial class AddRatings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceSubmissionRating",
                columns: table => new
                {
                    PriceSubmissionId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    Positive = table.Column<bool>(nullable: false),
                    SubmitedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceSubmissionRating", x => new { x.PriceSubmissionId, x.UserId });
                    table.ForeignKey(
                        name: "FK_PriceSubmissionRating_PriceSubmissions_PriceSubmissionId",
                        column: x => x.PriceSubmissionId,
                        principalTable: "PriceSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceSubmissionRating_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceSubmissionRating_UserId",
                table: "PriceSubmissionRating",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceSubmissionRating");
        }
    }
}
