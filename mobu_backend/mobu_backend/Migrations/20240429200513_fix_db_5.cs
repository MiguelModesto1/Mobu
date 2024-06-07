using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class fix_db_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Amizade_UtilizadorRegistado_AmigoFK",
                table: "Amizade");

            migrationBuilder.DropForeignKey(
                name: "FK_Amizade_UtilizadorRegistado_DonoListaAmigosFK",
                table: "Amizade");

            migrationBuilder.DropTable(
                name: "PedidosAmizade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RegistadosSalasChat",
                table: "RegistadosSalasChat");

            migrationBuilder.DropIndex(
                name: "IX_RegistadosSalasChat_UtilizadorFK",
                table: "RegistadosSalasChat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Amizade",
                table: "Amizade");

            migrationBuilder.DropIndex(
                name: "IX_Amizade_AmigoFK",
                table: "Amizade");

            migrationBuilder.DropColumn(
                name: "IDRegisto",
                table: "RegistadosSalasChat");

            migrationBuilder.DropColumn(
                name: "IdAmizade",
                table: "Amizade");

            migrationBuilder.RenameColumn(
                name: "DonoListaAmigosFK",
                table: "Amizade",
                newName: "RemetenteFK");

            migrationBuilder.RenameColumn(
                name: "AmigoFK",
                table: "Amizade",
                newName: "DestinatarioFK");

            migrationBuilder.RenameIndex(
                name: "IX_Amizade_DonoListaAmigosFK",
                table: "Amizade",
                newName: "IX_Amizade_RemetenteFK");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataPedido",
                table: "Amizade",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataResposta",
                table: "Amizade",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Desbloqueado",
                table: "Amizade",
                type: "bit",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RegistadosSalasChat",
                table: "RegistadosSalasChat",
                columns: new[] { "UtilizadorFK", "SalaFK" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Amizade",
                table: "Amizade",
                columns: new[] { "DestinatarioFK", "RemetenteFK" });

            migrationBuilder.AddForeignKey(
                name: "FK_Amizade_UtilizadorRegistado_DestinatarioFK",
                table: "Amizade",
                column: "DestinatarioFK",
                principalTable: "UtilizadorRegistado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Amizade_UtilizadorRegistado_RemetenteFK",
                table: "Amizade",
                column: "RemetenteFK",
                principalTable: "UtilizadorRegistado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Amizade_UtilizadorRegistado_DestinatarioFK",
                table: "Amizade");

            migrationBuilder.DropForeignKey(
                name: "FK_Amizade_UtilizadorRegistado_RemetenteFK",
                table: "Amizade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RegistadosSalasChat",
                table: "RegistadosSalasChat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Amizade",
                table: "Amizade");

            migrationBuilder.DropColumn(
                name: "DataPedido",
                table: "Amizade");

            migrationBuilder.DropColumn(
                name: "DataResposta",
                table: "Amizade");

            migrationBuilder.DropColumn(
                name: "Desbloqueado",
                table: "Amizade");

            migrationBuilder.RenameColumn(
                name: "RemetenteFK",
                table: "Amizade",
                newName: "DonoListaAmigosFK");

            migrationBuilder.RenameColumn(
                name: "DestinatarioFK",
                table: "Amizade",
                newName: "AmigoFK");

            migrationBuilder.RenameIndex(
                name: "IX_Amizade_RemetenteFK",
                table: "Amizade",
                newName: "IX_Amizade_DonoListaAmigosFK");

            migrationBuilder.AddColumn<int>(
                name: "IDRegisto",
                table: "RegistadosSalasChat",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "IdAmizade",
                table: "Amizade",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RegistadosSalasChat",
                table: "RegistadosSalasChat",
                column: "IDRegisto");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Amizade",
                table: "Amizade",
                column: "IdAmizade");

            migrationBuilder.CreateTable(
                name: "PedidosAmizade",
                columns: table => new
                {
                    IdPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonoListaPedidosFK = table.Column<int>(type: "int", nullable: false),
                    RemetenteFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidosAmizade", x => x.IdPedido);
                    table.ForeignKey(
                        name: "FK_PedidosAmizade_UtilizadorRegistado_DonoListaPedidosFK",
                        column: x => x.DonoListaPedidosFK,
                        principalTable: "UtilizadorRegistado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PedidosAmizade_UtilizadorRegistado_RemetenteFK",
                        column: x => x.RemetenteFK,
                        principalTable: "UtilizadorRegistado",
                        principalColumn: "IDUtilizador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistadosSalasChat_UtilizadorFK",
                table: "RegistadosSalasChat",
                column: "UtilizadorFK");

            migrationBuilder.CreateIndex(
                name: "IX_Amizade_AmigoFK",
                table: "Amizade",
                column: "AmigoFK");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosAmizade_DonoListaPedidosFK",
                table: "PedidosAmizade",
                column: "DonoListaPedidosFK");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosAmizade_RemetenteFK",
                table: "PedidosAmizade",
                column: "RemetenteFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Amizade_UtilizadorRegistado_AmigoFK",
                table: "Amizade",
                column: "AmigoFK",
                principalTable: "UtilizadorRegistado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Amizade_UtilizadorRegistado_DonoListaAmigosFK",
                table: "Amizade",
                column: "DonoListaAmigosFK",
                principalTable: "UtilizadorRegistado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
