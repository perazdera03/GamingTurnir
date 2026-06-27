using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GamingTurnir.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    KorisnikId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rola = table.Column<int>(type: "int", nullable: false),
                    DatumRegisdtracije = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.KorisnikId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Timovi",
                columns: table => new
                {
                    TimId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Opis = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DatumOsnivanja = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timovi", x => x.TimId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Turniri",
                columns: table => new
                {
                    TurnirId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Igrica = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DatumPocetka = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turniri", x => x.TurnirId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ClanoviTima",
                columns: table => new
                {
                    ClanTimaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    TimId = table.Column<int>(type: "int", nullable: false),
                    Uloga = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClanoviTima", x => x.ClanTimaId);
                    table.ForeignKey(
                        name: "FK_ClanoviTima_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClanoviTima_Timovi_TimId",
                        column: x => x.TimId,
                        principalTable: "Timovi",
                        principalColumn: "TimId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Mecevi",
                columns: table => new
                {
                    MecId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TurnirId = table.Column<int>(type: "int", nullable: false),
                    Tim1Id = table.Column<int>(type: "int", nullable: false),
                    Tim2Id = table.Column<int>(type: "int", nullable: false),
                    RezultatTim1 = table.Column<int>(type: "int", nullable: true),
                    RezultatTim2 = table.Column<int>(type: "int", nullable: true),
                    DatumMeca = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mecevi", x => x.MecId);
                    table.ForeignKey(
                        name: "FK_Mecevi_Timovi_Tim1Id",
                        column: x => x.Tim1Id,
                        principalTable: "Timovi",
                        principalColumn: "TimId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mecevi_Timovi_Tim2Id",
                        column: x => x.Tim2Id,
                        principalTable: "Timovi",
                        principalColumn: "TimId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mecevi_Turniri_TurnirId",
                        column: x => x.TurnirId,
                        principalTable: "Turniri",
                        principalColumn: "TurnirId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ClanoviTima_KorisnikId",
                table: "ClanoviTima",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_ClanoviTima_TimId",
                table: "ClanoviTima",
                column: "TimId");

            migrationBuilder.CreateIndex(
                name: "IX_Mecevi_Tim1Id",
                table: "Mecevi",
                column: "Tim1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Mecevi_Tim2Id",
                table: "Mecevi",
                column: "Tim2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Mecevi_TurnirId",
                table: "Mecevi",
                column: "TurnirId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClanoviTima");

            migrationBuilder.DropTable(
                name: "Mecevi");

            migrationBuilder.DropTable(
                name: "Korisnici");

            migrationBuilder.DropTable(
                name: "Timovi");

            migrationBuilder.DropTable(
                name: "Turniri");
        }
    }
}
