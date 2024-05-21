using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IOIT.Identity.Infrastructure.Migrator.Migrations
{
    public partial class addtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apartment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<long>(nullable: true),
                    UpdatedById = table.Column<long>(nullable: true),
                    ApartmentId = table.Column<int>(nullable: false),
                    FloorId = table.Column<int>(nullable: true),
                    TowerId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentMap",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<long>(nullable: true),
                    UpdatedById = table.Column<long>(nullable: true),
                    ApartmentId = table.Column<int>(nullable: true),
                    FloorId = table.Column<int>(nullable: true),
                    TowerId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    ResidentId = table.Column<long>(nullable: true),
                    RelationshipId = table.Column<int>(nullable: true),
                    DateRent = table.Column<DateTime>(nullable: true),
                    DateStart = table.Column<DateTime>(nullable: true),
                    DateEnd = table.Column<DateTime>(nullable: true),
                    Type = table.Column<byte>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentMap", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<long>(nullable: true),
                    UpdatedById = table.Column<long>(nullable: true),
                    DepartmentId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 500, nullable: false),
                    ProjectId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<long>(nullable: true),
                    UpdatedById = table.Column<long>(nullable: true),
                    FullName = table.Column<string>(maxLength: 200, nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Avata = table.Column<string>(maxLength: 1024, nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    ProjectName = table.Column<string>(maxLength: 500, nullable: true),
                    PositionId = table.Column<int>(nullable: true),
                    DepartmentId = table.Column<int>(nullable: true),
                    DepartmentName = table.Column<string>(maxLength: 200, nullable: true),
                    Birthday = table.Column<DateTime>(nullable: true),
                    CardId = table.Column<string>(maxLength: 255, nullable: true),
                    Phone = table.Column<string>(maxLength: 50, nullable: true, defaultValue: ""),
                    Email = table.Column<string>(maxLength: 255, nullable: true),
                    Address = table.Column<string>(maxLength: 1000, nullable: true),
                    Note = table.Column<string>(maxLength: 2000, nullable: true),
                    TypeEmployee = table.Column<byte>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeMap",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<long>(nullable: true),
                    UpdatedById = table.Column<long>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: true),
                    TowerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeMap", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Floor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<long>(nullable: true),
                    UpdatedById = table.Column<long>(nullable: true),
                    FloorId = table.Column<int>(nullable: false),
                    TowerId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Floor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Function",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<long>(nullable: true),
                    UpdatedById = table.Column<long>(nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    FunctionParentId = table.Column<int>(nullable: false),
                    Url = table.Column<string>(maxLength: 200, nullable: true),
                    Note = table.Column<string>(maxLength: 2000, nullable: true),
                    Location = table.Column<int>(nullable: true),
                    Icon = table.Column<string>(maxLength: 500, nullable: true),
                    IsParamRoute = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Function", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<long>(nullable: true),
                    UpdatedById = table.Column<long>(nullable: true),
                    Name = table.Column<string>(maxLength: 500, nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    TowerId = table.Column<int>(nullable: true),
                    LevelId = table.Column<int>(nullable: true),
                    Note = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Project",
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
                    OneSId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resident",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<long>(nullable: true),
                    UpdatedById = table.Column<long>(nullable: true),
                    OneSid = table.Column<int>(nullable: true),
                    FullName = table.Column<string>(maxLength: 500, nullable: true),
                    Birthday = table.Column<DateTime>(nullable: true),
                    CardId = table.Column<string>(maxLength: 255, nullable: true),
                    DateId = table.Column<DateTime>(nullable: true),
                    TypeCardId = table.Column<int>(nullable: true),
                    AddressId = table.Column<string>(maxLength: 500, nullable: true),
                    Phone = table.Column<string>(maxLength: 50, nullable: true, defaultValue: ""),
                    Email = table.Column<string>(maxLength: 500, nullable: true),
                    Address = table.Column<string>(type: "ntext", nullable: true),
                    Avata = table.Column<string>(type: "ntext", nullable: true),
                    Sex = table.Column<string>(maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "ntext", nullable: true),
                    DateRent = table.Column<DateTime>(nullable: true),
                    Type = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resident", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<long>(nullable: true),
                    UpdatedById = table.Column<long>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 500, nullable: false),
                    LevelRole = table.Column<byte>(nullable: true),
                    Note = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tower",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<long>(nullable: true),
                    UpdatedById = table.Column<long>(nullable: true),
                    TowerId = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tower", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeAttributeItem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<long>(nullable: true),
                    UpdatedById = table.Column<long>(nullable: true),
                    TypeAttributeItemId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeAttributeItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<long>(nullable: true),
                    UpdatedById = table.Column<long>(nullable: true),
                    FullName = table.Column<string>(maxLength: 255, nullable: false),
                    UserName = table.Column<string>(maxLength: 255, nullable: true),
                    Password = table.Column<string>(maxLength: 200, nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Avata = table.Column<string>(type: "ntext", nullable: true),
                    PositionId = table.Column<int>(nullable: true),
                    DepartmentId = table.Column<int>(nullable: true),
                    CardId = table.Column<string>(maxLength: 50, nullable: true),
                    Phone = table.Column<string>(maxLength: 50, nullable: true, defaultValue: ""),
                    Email = table.Column<string>(maxLength: 1000, nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Address = table.Column<string>(maxLength: 500, nullable: true),
                    KeyRandom = table.Column<string>(maxLength: 20, nullable: true),
                    TypeThird = table.Column<byte>(nullable: true),
                    UserMapId = table.Column<long>(nullable: true),
                    Type = table.Column<byte>(nullable: true),
                    LastLoginAt = table.Column<DateTime>(nullable: true),
                    RegEmail = table.Column<string>(maxLength: 50, nullable: true),
                    RoleMax = table.Column<int>(nullable: true),
                    RoleLevel = table.Column<byte>(nullable: true),
                    IsRoleGroup = table.Column<bool>(nullable: true),
                    IsPhoneConfirm = table.Column<bool>(nullable: true),
                    IsEmailConfirm = table.Column<bool>(nullable: true),
                    RegisterCode = table.Column<string>(maxLength: 10, nullable: true),
                    CountLogin = table.Column<int>(nullable: true),
                    LanguageId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    IsDeletedByGuest = table.Column<int>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<long>(nullable: true),
                    UpdatedById = table.Column<long>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    TargetId = table.Column<int>(nullable: true),
                    TargetType = table.Column<byte>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMapping", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<long>(nullable: true),
                    UpdatedById = table.Column<long>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FunctionRole",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<long>(nullable: true),
                    UpdatedById = table.Column<long>(nullable: true),
                    TargetId = table.Column<long>(nullable: false),
                    FunctionId = table.Column<int>(nullable: false),
                    ActiveKey = table.Column<string>(maxLength: 20, nullable: true),
                    Type = table.Column<byte>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FunctionRole_Function_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Function",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FunctionRole_FunctionId",
                table: "FunctionRole",
                column: "FunctionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Apartment");

            migrationBuilder.DropTable(
                name: "ApartmentMap");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "EmployeeMap");

            migrationBuilder.DropTable(
                name: "Floor");

            migrationBuilder.DropTable(
                name: "FunctionRole");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Resident");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Tower");

            migrationBuilder.DropTable(
                name: "TypeAttributeItem");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UserMapping");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Function");
        }
    }
}
