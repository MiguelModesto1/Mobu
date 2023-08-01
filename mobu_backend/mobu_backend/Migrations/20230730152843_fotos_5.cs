using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class fotos_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fotografia_Admin_Admin_AdminFK",
                table: "Fotografia_Admin");

            migrationBuilder.DropForeignKey(
                name: "FK_Fotografia_Anonimo_Utilizador_Anonimo_AnonimoFK",
                table: "Fotografia_Anonimo");

            migrationBuilder.DropForeignKey(
                name: "FK_Fotografia_Registado_Utilizador_Registado_RegistadoFK",
                table: "Fotografia_Registado");

            migrationBuilder.DropIndex(
                name: "IX_Fotografia_Registado_RegistadoFK",
                table: "Fotografia_Registado");

            migrationBuilder.DropIndex(
                name: "IX_Fotografia_Anonimo_AnonimoFK",
                table: "Fotografia_Anonimo");

            migrationBuilder.DropIndex(
                name: "IX_Fotografia_Admin_AdminFK",
                table: "Fotografia_Admin");

            migrationBuilder.DropColumn(
                name: "RegistadoFK",
                table: "Fotografia_Registado");

            migrationBuilder.DropColumn(
                name: "AnonimoFK",
                table: "Fotografia_Anonimo");

            migrationBuilder.DropColumn(
                name: "AdminFK",
                table: "Fotografia_Admin");

            migrationBuilder.AddColumn<int>(
                name: "IDFotografia",
                table: "Utilizador_Registado",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_Utilizador_Registado_IDFotografia",
                table: "Utilizador_Registado",
                column: "IDFotografia");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizador_Registado_Fotografia_Registado_IDFotografia",
                table: "Utilizador_Registado",
                column: "IDFotografia",
                principalTable: "Fotografia_Registado",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admin_Fotografia_Admin_IDFotografia",
                table: "Admin");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizador_Anonimo_Fotografia_Anonimo_IDFotografia",
                table: "Utilizador_Anonimo");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizador_Registado_Fotografia_Registado_IDFotografia",
                table: "Utilizador_Registado");

            migrationBuilder.DropIndex(
                name: "IX_Utilizador_Registado_IDFotografia",
                table: "Utilizador_Registado");

            migrationBuilder.DropIndex(
                name: "IX_Utilizador_Anonimo_IDFotografia",
                table: "Utilizador_Anonimo");

            migrationBuilder.DropIndex(
                name: "IX_Admin_IDFotografia",
                table: "Admin");

            migrationBuilder.DropColumn(
                name: "IDFotografia",
                table: "Utilizador_Registado");

            migrationBuilder.DropColumn(
                name: "IDFotografia",
                table: "Utilizador_Anonimo");

            migrationBuilder.DropColumn(
                name: "IDFotografia",
                table: "Admin");

            migrationBuilder.AddColumn<int>(
                name: "RegistadoFK",
                table: "Fotografia_Registado",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnonimoFK",
                table: "Fotografia_Anonimo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AdminFK",
                table: "Fotografia_Admin",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Fotografia_Registado_RegistadoFK",
                table: "Fotografia_Registado",
                column: "RegistadoFK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fotografia_Anonimo_AnonimoFK",
                table: "Fotografia_Anonimo",
                column: "AnonimoFK",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fotografia_Admin_AdminFK",
                table: "Fotografia_Admin",
                column: "AdminFK",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Fotografia_Admin_Admin_AdminFK",
                table: "Fotografia_Admin",
                column: "AdminFK",
                principalTable: "Admin",
                principalColumn: "IDAdmin",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fotografia_Anonimo_Utilizador_Anonimo_AnonimoFK",
                table: "Fotografia_Anonimo",
                column: "AnonimoFK",
                principalTable: "Utilizador_Anonimo",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fotografia_Registado_Utilizador_Registado_RegistadoFK",
                table: "Fotografia_Registado",
                column: "RegistadoFK",
                principalTable: "Utilizador_Registado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
