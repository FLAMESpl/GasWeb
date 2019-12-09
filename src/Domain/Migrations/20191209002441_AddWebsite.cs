using Microsoft.EntityFrameworkCore.Migrations;

namespace GasWeb.Domain.Migrations
{
    public partial class AddWebsite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WebsiteAddress",
                table: "GasStations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WebsiteAddress",
                table: "GasStations");
        }
    }
}
