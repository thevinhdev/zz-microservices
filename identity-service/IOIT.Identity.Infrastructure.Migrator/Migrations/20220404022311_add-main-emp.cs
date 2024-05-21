using Microsoft.EntityFrameworkCore.Migrations;

namespace IOIT.Identity.Infrastructure.Migrator.Migrations
{
    public partial class addmainemp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "Employee",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "Employee");
        }
    }
}
