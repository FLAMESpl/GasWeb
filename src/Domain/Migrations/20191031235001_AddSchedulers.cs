using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GasWeb.Domain.Migrations
{
    public partial class AddSchedulers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Schedulers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedByUserId = table.Column<long>(nullable: false),
                    ModifiedByUserId = table.Column<long>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: false),
                    FranchiseId = table.Column<long>(nullable: false),
                    Interval = table.Column<TimeSpan>(nullable: false),
                    StartedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedulers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedulers_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedulers_Franchises_FranchiseId",
                        column: x => x.FranchiseId,
                        principalTable: "Franchises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedulers_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedulers_CreatedByUserId",
                table: "Schedulers",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedulers_FranchiseId",
                table: "Schedulers",
                column: "FranchiseId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedulers_ModifiedByUserId",
                table: "Schedulers",
                column: "ModifiedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Schedulers");
        }
    }
}
