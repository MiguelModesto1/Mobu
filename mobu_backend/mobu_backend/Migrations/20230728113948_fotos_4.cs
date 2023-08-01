using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class fotos_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Fotografia_Registado_RegistadoFK",
                table: "Fotografia_Registado");

            migrationBuilder.DropIndex(
                name: "IX_Fotografia_Anonimo_AnonimoFK",
                table: "Fotografia_Anonimo");

            migrationBuilder.DropIndex(
                name: "IX_Fotografia_Admin_AdminFK",
                table: "Fotografia_Admin");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Fotografia_Registado_RegistadoFK",
                table: "Fotografia_Registado");

            migrationBuilder.DropIndex(
                name: "IX_Fotografia_Anonimo_AnonimoFK",
                table: "Fotografia_Anonimo");

            migrationBuilder.DropIndex(
                name: "IX_Fotografia_Admin_AdminFK",
                table: "Fotografia_Admin");

            migrationBuilder.CreateIndex(
                name: "IX_Fotografia_Registado_RegistadoFK",
                table: "Fotografia_Registado",
                column: "RegistadoFK");

            migrationBuilder.CreateIndex(
                name: "IX_Fotografia_Anonimo_AnonimoFK",
                table: "Fotografia_Anonimo",
                column: "AnonimoFK");

            migrationBuilder.CreateIndex(
                name: "IX_Fotografia_Admin_AdminFK",
                table: "Fotografia_Admin",
                column: "AdminFK");
        }
    }
}
