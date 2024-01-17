using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StaffScreening.Migrations
{
    public partial class AddScreening : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Screenings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Question1 = table.Column<bool>(type: "bit", nullable: false),
                    Question2 = table.Column<bool>(type: "bit", nullable: false),
                    Question3 = table.Column<bool>(type: "bit", nullable: false),
                    DateCompleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PassedScreening = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screenings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Screenings");
        }
    }
}
