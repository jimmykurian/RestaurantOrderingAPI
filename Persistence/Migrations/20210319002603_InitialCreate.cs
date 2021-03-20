using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Desserts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MealType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Desserts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Drinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MealType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MainDishes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MealType = table.Column<int>(nullable: false),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainDishes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SidesDishes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MealType = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SidesDishes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Desserts");

            migrationBuilder.DropTable(
                name: "Drinks");

            migrationBuilder.DropTable(
                name: "MainDishes");

            migrationBuilder.DropTable(
                name: "SidesDishes");
        }
    }
}
