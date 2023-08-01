using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class fotos_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fotografia",
                table: "Utilizador_Registado");

            migrationBuilder.DropColumn(
                name: "Fotografia",
                table: "Utilizador_Anonimo");

            migrationBuilder.DropColumn(
                name: "Fotografia",
                table: "Admin");

            migrationBuilder.CreateTable(
                name: "Fotografia_Admin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeFicheiro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Local = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataFotografia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdminFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotografia_Admin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fotografia_Admin_Admin_AdminFK",
                        column: x => x.AdminFK,
                        principalTable: "Admin",
                        principalColumn: "IDAdmin",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fotografia_Anonimo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeFicheiro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Local = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataFotografia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnonimoFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotografia_Anonimo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fotografia_Anonimo_Utilizador_Anonimo_AnonimoFK",
                        column: x => x.AnonimoFK,
                        principalTable: "Utilizador_Anonimo",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fotografia_Registado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeFicheiro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Local = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataFotografia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegistadoFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotografia_Registado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fotografia_Registado_Utilizador_Registado_RegistadoFK",
                        column: x => x.RegistadoFK,
                        principalTable: "Utilizador_Registado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fotografia_Admin_AdminFK",
                table: "Fotografia_Admin",
                column: "AdminFK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fotografia_Anonimo_AnonimoFK",
                table: "Fotografia_Anonimo",
                column: "AnonimoFK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fotografia_Registado_RegistadoFK",
                table: "Fotografia_Registado",
                column: "RegistadoFK",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fotografia_Admin");

            migrationBuilder.DropTable(
                name: "Fotografia_Anonimo");

            migrationBuilder.DropTable(
                name: "Fotografia_Registado");

            migrationBuilder.AddColumn<string>(
                name: "Fotografia",
                table: "Utilizador_Registado",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fotografia",
                table: "Utilizador_Anonimo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fotografia",
                table: "Admin",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
