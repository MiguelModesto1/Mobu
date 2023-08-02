using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class fotos_11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admin_Fotografia_Admin_IDFotografia",
                table: "Admin");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizador_Anonimo_Fotografia_Anonimo_IDFotografia",
                table: "Utilizador_Anonimo");

            migrationBuilder.DropTable(
                name: "Fotografia_Admin");

            migrationBuilder.DropTable(
                name: "Fotografia_Anonimo");

            migrationBuilder.DropIndex(
                name: "IX_Utilizador_Anonimo_IDFotografia",
                table: "Utilizador_Anonimo");

            migrationBuilder.DropIndex(
                name: "IX_Admin_IDFotografia",
                table: "Admin");

            migrationBuilder.DropColumn(
                name: "IDFotografia",
                table: "Utilizador_Anonimo");

            migrationBuilder.DropColumn(
                name: "IDFotografia",
                table: "Admin");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataJuncao",
                table: "Utilizador_Registado",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFotografia",
                table: "Admin",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataJuncao",
                table: "Admin",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "NomeFotografia",
                table: "Admin",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataJuncao",
                table: "Utilizador_Registado");

            migrationBuilder.DropColumn(
                name: "DataFotografia",
                table: "Admin");

            migrationBuilder.DropColumn(
                name: "DataJuncao",
                table: "Admin");

            migrationBuilder.DropColumn(
                name: "NomeFotografia",
                table: "Admin");

            migrationBuilder.AddColumn<int>(
                name: "IDFotografia",
                table: "Utilizador_Anonimo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IDFotografia",
                table: "Admin",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Fotografia_Admin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataFotografia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Local = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomeFicheiro = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotografia_Admin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fotografia_Anonimo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataFotografia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Local = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomeFicheiro = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotografia_Anonimo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Utilizador_Anonimo_IDFotografia",
                table: "Utilizador_Anonimo",
                column: "IDFotografia");

            migrationBuilder.CreateIndex(
                name: "IX_Admin_IDFotografia",
                table: "Admin",
                column: "IDFotografia");

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_Fotografia_Admin_IDFotografia",
                table: "Admin",
                column: "IDFotografia",
                principalTable: "Fotografia_Admin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizador_Anonimo_Fotografia_Anonimo_IDFotografia",
                table: "Utilizador_Anonimo",
                column: "IDFotografia",
                principalTable: "Fotografia_Anonimo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
