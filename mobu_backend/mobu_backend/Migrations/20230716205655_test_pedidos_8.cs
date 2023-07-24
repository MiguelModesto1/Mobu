using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class test_pedidos_8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Amizade_Utilizador_Registado_DestinatarioFK",
                table: "Pedidos_Amizade");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Amizade_Utilizador_Registado_DestinatarioFK",
                table: "Pedidos_Amizade",
                column: "DestinatarioFK",
                principalTable: "Utilizador_Registado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Amizade_Utilizador_Registado_DestinatarioFK",
                table: "Pedidos_Amizade");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Amizade_Utilizador_Registado_DestinatarioFK",
                table: "Pedidos_Amizade",
                column: "DestinatarioFK",
                principalTable: "Utilizador_Registado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
