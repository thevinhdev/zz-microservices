using Microsoft.EntityFrameworkCore.Migrations;

namespace IOIT.Identity.Infrastructure.Migrator.Migrations
{
    public partial class addcountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Resident",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Resident");
        }
    }
}
