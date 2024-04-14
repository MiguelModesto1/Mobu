using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class change_database_structure_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amigo");

            migrationBuilder.DropTable(
                name: "DestinatarioPedidosAmizade");

            migrationBuilder.AddColumn<int>(
                name: "DonoListaAmigosId",
                table: "UtilizadorRegistado",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DonoListaDestinatáriosId",
                table: "UtilizadorRegistado",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UtilizadorRegistado_DonoListaAmigosId",
                table: "UtilizadorRegistado",
                column: "DonoListaAmigosId");

            migrationBuilder.CreateIndex(
                name: "IX_UtilizadorRegistado_DonoListaDestinatáriosId",
                table: "UtilizadorRegistado",
                column: "DonoListaDestinatáriosId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UtilizadorRegistado_UtilizadorRegistado_DonoListaAmigosId",
                table: "UtilizadorRegistado");

            migrationBuilder.DropForeignKey(
                name: "FK_UtilizadorRegistado_UtilizadorRegistado_DonoListaDestinatáriosId",
                table: "UtilizadorRegistado");

            migrationBuilder.DropIndex(
                name: "IX_UtilizadorRegistado_DonoListaAmigosId",
                table: "UtilizadorRegistado");

            migrationBuilder.DropIndex(
                name: "IX_UtilizadorRegistado_DonoListaDestinatáriosId",
                table: "UtilizadorRegistado");

            migrationBuilder.DropColumn(
                name: "DonoListaAmigosId",
                table: "UtilizadorRegistado");

            migrationBuilder.DropColumn(
                name: "DonoListaDestinatáriosId",
                table: "UtilizadorRegistado");

            migrationBuilder.CreateTable(
                name: "Amigo",
                columns: table => new
                {
                    IDAmizade = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonoListaFK = table.Column<int>(type: "int", nullable: false),
                    Bloqueado = table.Column<bool>(type: "bit", nullable: false),
                    IDAmigo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amigo", x => x.IDAmizade);
                    table.ForeignKey(
                        name: "FK_Amigo_UtilizadorRegistado_DonoListaFK",
                        column: x => x.DonoListaFK,
                        principalTable: "UtilizadorRegistado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DestinatarioPedidosAmizade",
                columns: table => new
                {
                    IDPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RemetenteFK = table.Column<int>(type: "int", nullable: false),
                    DataHoraPedido = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IDDestinatarioPedido = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DestinatarioPedidosAmizade", x => x.IDPedido);
                    table.ForeignKey(
                        name: "FK_DestinatarioPedidosAmizade_UtilizadorRegistado_RemetenteFK",
                        column: x => x.RemetenteFK,
                        principalTable: "UtilizadorRegistado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Amigo_DonoListaFK",
                table: "Amigo",
                column: "DonoListaFK");

            migrationBuilder.CreateIndex(
                name: "IX_DestinatarioPedidosAmizade_RemetenteFK",
                table: "DestinatarioPedidosAmizade",
                column: "RemetenteFK");
        }
    }
}
