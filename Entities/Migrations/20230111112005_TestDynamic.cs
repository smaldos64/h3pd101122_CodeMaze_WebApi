using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class TestDynamic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestDynamics",
                columns: table => new
                {
                    TestDynamicId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestDynamicString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestDynamicBool = table.Column<bool>(type: "bit", nullable: false),
                    TestDynamicDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TestDynamicInt = table.Column<int>(type: "int", nullable: false),
                    TestDynamicFloat = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestDynamics", x => x.TestDynamicId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestDynamics");
        }
    }
}
