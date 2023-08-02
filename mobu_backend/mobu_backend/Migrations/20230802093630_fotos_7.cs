using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class fotos_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Utilizador_Registado_Fotografia_Registado_IDFotografia",
                table: "Utilizador_Registado");

            migrationBuilder.DropIndex(
                name: "IX_Utilizador_Registado_IDFotografia",
                table: "Utilizador_Registado");

            migrationBuilder.DropColumn(
                name: "IDFotografia",
                table: "Utilizador_Registado");

            migrationBuilder.AddColumn<int>(
                name: "RegistadoFK",
                table: "Fotografia_Registado",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fotografia_Registado_RegistadoFK",
                table: "Fotografia_Registado",
                column: "RegistadoFK",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Fotografia_Registado_Utilizador_Registado_RegistadoFK",
                table: "Fotografia_Registado",
                column: "RegistadoFK",
                principalTable: "Utilizador_Registado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fotografia_Registado_Utilizador_Registado_RegistadoFK",
                table: "Fotografia_Registado");

            migrationBuilder.DropIndex(
                name: "IX_Fotografia_Registado_RegistadoFK",
                table: "Fotografia_Registado");

            migrationBuilder.DropColumn(
                name: "RegistadoFK",
                table: "Fotografia_Registado");

            migrationBuilder.AddColumn<int>(
                name: "IDFotografia",
                table: "Utilizador_Registado",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Utilizador_Registado_IDFotografia",
                table: "Utilizador_Registado",
                column: "IDFotografia");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizador_Registado_Fotografia_Registado_IDFotografia",
                table: "Utilizador_Registado",
                column: "IDFotografia",
                principalTable: "Fotografia_Registado",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
