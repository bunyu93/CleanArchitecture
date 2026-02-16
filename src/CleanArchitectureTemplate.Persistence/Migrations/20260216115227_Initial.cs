using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CleanArchitectureTemplate.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "weather");

            migrationBuilder.CreateTable(
                name: "forecast",
                schema: "weather",
                columns: table => new
                {
                    id = table.Column<int>(type: "Integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date = table.Column<DateTime>(type: "Date", nullable: false),
                    fahrenheit = table.Column<int>(type: "Integer", nullable: false),
                    celsius = table.Column<int>(type: "Integer", nullable: false),
                    summary = table.Column<string>(type: "Text", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_forecast", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "forecast",
                schema: "weather");
        }
    }
}
