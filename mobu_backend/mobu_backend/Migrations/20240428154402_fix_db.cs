using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class fix_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UtilizadorRegistado_UtilizadorRegistado_DonoListaAmigosId",
                table: "UtilizadorRegistado");

            migrationBuilder.DropForeignKey(
                name: "FK_UtilizadorRegistado_UtilizadorRegistado_DonoListaDestinatáriosId",
                table: "UtilizadorRegistado");

            migrationBuilder.RenameColumn(
                name: "DonoListaDestinatáriosId",
                table: "UtilizadorRegistado",
                newName: "DonoListaPedidosFK");

            migrationBuilder.RenameColumn(
                name: "DonoListaAmigosId",
                table: "UtilizadorRegistado",
                newName: "DonoListaAmigosFK");

            migrationBuilder.RenameIndex(
                name: "IX_UtilizadorRegistado_DonoListaDestinatáriosId",
                table: "UtilizadorRegistado",
                newName: "IX_UtilizadorRegistado_DonoListaPedidosFK");

            migrationBuilder.RenameIndex(
                name: "IX_UtilizadorRegistado_DonoListaAmigosId",
                table: "UtilizadorRegistado",
                newName: "IX_UtilizadorRegistado_DonoListaAmigosFK");

            migrationBuilder.AddForeignKey(
                name: "FK_UtilizadorRegistado_UtilizadorRegistado_DonoListaAmigosFK",
                table: "UtilizadorRegistado",
                column: "DonoListaAmigosFK",
                principalTable: "UtilizadorRegistado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UtilizadorRegistado_UtilizadorRegistado_DonoListaPedidosFK",
                table: "UtilizadorRegistado",
                column: "DonoListaPedidosFK",
                principalTable: "UtilizadorRegistado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UtilizadorRegistado_UtilizadorRegistado_DonoListaAmigosFK",
                table: "UtilizadorRegistado");

            migrationBuilder.DropForeignKey(
                name: "FK_UtilizadorRegistado_UtilizadorRegistado_DonoListaPedidosFK",
                table: "UtilizadorRegistado");

            migrationBuilder.RenameColumn(
                name: "DonoListaPedidosFK",
                table: "UtilizadorRegistado",
                newName: "DonoListaDestinatáriosId");

            migrationBuilder.RenameColumn(
                name: "DonoListaAmigosFK",
                table: "UtilizadorRegistado",
                newName: "DonoListaAmigosId");

            migrationBuilder.RenameIndex(
                name: "IX_UtilizadorRegistado_DonoListaPedidosFK",
                table: "UtilizadorRegistado",
                newName: "IX_UtilizadorRegistado_DonoListaDestinatáriosId");

            migrationBuilder.RenameIndex(
                name: "IX_UtilizadorRegistado_DonoListaAmigosFK",
                table: "UtilizadorRegistado",
                newName: "IX_UtilizadorRegistado_DonoListaAmigosId");

            migrationBuilder.AddForeignKey(
                name: "FK_UtilizadorRegistado_UtilizadorRegistado_DonoListaAmigosId",
                table: "UtilizadorRegistado",
                column: "DonoListaAmigosId",
                principalTable: "UtilizadorRegistado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UtilizadorRegistado_UtilizadorRegistado_DonoListaDestinatáriosId",
                table: "UtilizadorRegistado",
                column: "DonoListaDestinatáriosId",
                principalTable: "UtilizadorRegistado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
