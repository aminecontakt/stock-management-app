using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockManagement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Produits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nom = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Reference = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Categorie = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Quantite = table.Column<int>(type: "integer", nullable: false),
                    Prix = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    StockMinimum = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DateAjout = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produits", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Produits",
                columns: new[] { "Id", "Categorie", "DateAjout", "Description", "Nom", "Prix", "Quantite", "Reference", "StockMinimum" },
                values: new object[,]
                {
                    { 1, "Fournitures", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Stylo Bic", 2.50m, 150, "STY-001", 20 },
                    { 2, "Fournitures", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ramette de papier A4", 45.00m, 30, "PAP-001", 10 },
                    { 3, "Informatique", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Clavier USB", 180.00m, 3, "INF-001", 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produits");
        }
    }
}
