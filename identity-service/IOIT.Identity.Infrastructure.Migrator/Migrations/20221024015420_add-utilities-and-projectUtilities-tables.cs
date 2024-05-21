using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IOIT.Identity.Infrastructure.Migrator.Migrations
{
    public partial class addutilitiesandprojectUtilitiestables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectUtilities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<long>(nullable: true),
                    UpdatedById = table.Column<long>(nullable: true),
                    ProjectId = table.Column<int>(nullable: false),
                    UtilitiesId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    ActivedDate = table.Column<DateTime>(nullable: true),
                    ExpiredDate = table.Column<DateTime>(nullable: true),
                    Note = table.Column<string>(nullable: false, defaultValue: ""),
                    CreatedBy = table.Column<string>(nullable: false, defaultValue: ""),
                    UpdatedBy = table.Column<string>(nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUtilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Utilities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<long>(nullable: true),
                    UpdatedById = table.Column<long>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Type = table.Column<int>(nullable: false, defaultValue: 2),
                    Url = table.Column<string>(maxLength: 512, nullable: false, defaultValue: ""),
                    Icon = table.Column<string>(maxLength: 512, nullable: false, defaultValue: ""),
                    Description = table.Column<string>(maxLength: 1024, nullable: false, defaultValue: ""),
                    Order = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilities", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectUtilities");

            migrationBuilder.DropTable(
                name: "Utilities");
        }
    }
}
