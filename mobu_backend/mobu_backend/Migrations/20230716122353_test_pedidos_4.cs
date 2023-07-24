using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class test_pedidos_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mensagem_Salas_Chat_SalaFK",
                table: "mensagem");

            migrationBuilder.DropForeignKey(
                name: "FK_mensagem_Utilizador_Registado_RemetenteFK",
                table: "mensagem");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Amizade_Utilizador_Registado_Utilizador_RegistadoIDUtilizador",
                table: "Pedidos_Amizade");

            migrationBuilder.DropForeignKey(
                name: "FK_registados_Salas_Chat_Salas_Chat_SalaFK",
                table: "registados_Salas_Chat");

            migrationBuilder.DropForeignKey(
                name: "FK_registados_Salas_Chat_Utilizador_Registado_UtilizadorFK",
                table: "registados_Salas_Chat");

            migrationBuilder.DropForeignKey(
                name: "FK_registados_Salas_Jogo_Sala_Jogo_1_Contra_1_SalaFK",
                table: "registados_Salas_Jogo");

            migrationBuilder.DropForeignKey(
                name: "FK_registados_Salas_Jogo_Utilizador_Registado_UtilizadorFK",
                table: "registados_Salas_Jogo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_registados_Salas_Jogo",
                table: "registados_Salas_Jogo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_registados_Salas_Chat",
                table: "registados_Salas_Chat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pedidos_Amizade",
                table: "Pedidos_Amizade");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_Amizade_Utilizador_RegistadoIDUtilizador",
                table: "Pedidos_Amizade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mensagem",
                table: "mensagem");

            migrationBuilder.DropColumn(
                name: "Utilizador_RegistadoIDUtilizador",
                table: "Pedidos_Amizade");

            migrationBuilder.RenameTable(
                name: "registados_Salas_Jogo",
                newName: "Registados_Salas_Jogo");

            migrationBuilder.RenameTable(
                name: "registados_Salas_Chat",
                newName: "Registados_Salas_Chat");

            migrationBuilder.RenameTable(
                name: "mensagem",
                newName: "Mensagem");

            migrationBuilder.RenameIndex(
                name: "IX_registados_Salas_Jogo_SalaFK",
                table: "Registados_Salas_Jogo",
                newName: "IX_Registados_Salas_Jogo_SalaFK");

            migrationBuilder.RenameIndex(
                name: "IX_registados_Salas_Chat_SalaFK",
                table: "Registados_Salas_Chat",
                newName: "IX_Registados_Salas_Chat_SalaFK");

            migrationBuilder.RenameIndex(
                name: "IX_mensagem_SalaFK",
                table: "Mensagem",
                newName: "IX_Mensagem_SalaFK");

            migrationBuilder.RenameIndex(
                name: "IX_mensagem_RemetenteFK",
                table: "Mensagem",
                newName: "IX_Mensagem_RemetenteFK");

            migrationBuilder.AddColumn<int>(
                name: "DestinatarioFK",
                table: "Pedidos_Amizade",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Registados_Salas_Jogo",
                table: "Registados_Salas_Jogo",
                columns: new[] { "UtilizadorFK", "SalaFK" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Registados_Salas_Chat",
                table: "Registados_Salas_Chat",
                columns: new[] { "UtilizadorFK", "SalaFK" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pedidos_Amizade",
                table: "Pedidos_Amizade",
                columns: new[] { "RemetenteFK", "DestinatarioFK" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mensagem",
                table: "Mensagem",
                columns: new[] { "IDMensagem", "SalaFK" });

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_Amizade_DestinatarioFK",
                table: "Pedidos_Amizade",
                column: "DestinatarioFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Mensagem_Salas_Chat_SalaFK",
                table: "Mensagem",
                column: "SalaFK",
                principalTable: "Salas_Chat",
                principalColumn: "IDSala",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mensagem_Utilizador_Registado_RemetenteFK",
                table: "Mensagem",
                column: "RemetenteFK",
                principalTable: "Utilizador_Registado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Amizade_Utilizador_Registado_DestinatarioFK",
                table: "Pedidos_Amizade",
                column: "DestinatarioFK",
                principalTable: "Utilizador_Registado",
                principalColumn: "IDUtilizador");

            migrationBuilder.AddForeignKey(
                name: "FK_Registados_Salas_Chat_Salas_Chat_SalaFK",
                table: "Registados_Salas_Chat",
                column: "SalaFK",
                principalTable: "Salas_Chat",
                principalColumn: "IDSala",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registados_Salas_Chat_Utilizador_Registado_UtilizadorFK",
                table: "Registados_Salas_Chat",
                column: "UtilizadorFK",
                principalTable: "Utilizador_Registado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registados_Salas_Jogo_Sala_Jogo_1_Contra_1_SalaFK",
                table: "Registados_Salas_Jogo",
                column: "SalaFK",
                principalTable: "Sala_Jogo_1_Contra_1",
                principalColumn: "IDSala",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registados_Salas_Jogo_Utilizador_Registado_UtilizadorFK",
                table: "Registados_Salas_Jogo",
                column: "UtilizadorFK",
                principalTable: "Utilizador_Registado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mensagem_Salas_Chat_SalaFK",
                table: "Mensagem");

            migrationBuilder.DropForeignKey(
                name: "FK_Mensagem_Utilizador_Registado_RemetenteFK",
                table: "Mensagem");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Amizade_Utilizador_Registado_DestinatarioFK",
                table: "Pedidos_Amizade");

            migrationBuilder.DropForeignKey(
                name: "FK_Registados_Salas_Chat_Salas_Chat_SalaFK",
                table: "Registados_Salas_Chat");

            migrationBuilder.DropForeignKey(
                name: "FK_Registados_Salas_Chat_Utilizador_Registado_UtilizadorFK",
                table: "Registados_Salas_Chat");

            migrationBuilder.DropForeignKey(
                name: "FK_Registados_Salas_Jogo_Sala_Jogo_1_Contra_1_SalaFK",
                table: "Registados_Salas_Jogo");

            migrationBuilder.DropForeignKey(
                name: "FK_Registados_Salas_Jogo_Utilizador_Registado_UtilizadorFK",
                table: "Registados_Salas_Jogo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Registados_Salas_Jogo",
                table: "Registados_Salas_Jogo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Registados_Salas_Chat",
                table: "Registados_Salas_Chat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pedidos_Amizade",
                table: "Pedidos_Amizade");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_Amizade_DestinatarioFK",
                table: "Pedidos_Amizade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mensagem",
                table: "Mensagem");

            migrationBuilder.DropColumn(
                name: "DestinatarioFK",
                table: "Pedidos_Amizade");

            migrationBuilder.RenameTable(
                name: "Registados_Salas_Jogo",
                newName: "registados_Salas_Jogo");

            migrationBuilder.RenameTable(
                name: "Registados_Salas_Chat",
                newName: "registados_Salas_Chat");

            migrationBuilder.RenameTable(
                name: "Mensagem",
                newName: "mensagem");

            migrationBuilder.RenameIndex(
                name: "IX_Registados_Salas_Jogo_SalaFK",
                table: "registados_Salas_Jogo",
                newName: "IX_registados_Salas_Jogo_SalaFK");

            migrationBuilder.RenameIndex(
                name: "IX_Registados_Salas_Chat_SalaFK",
                table: "registados_Salas_Chat",
                newName: "IX_registados_Salas_Chat_SalaFK");

            migrationBuilder.RenameIndex(
                name: "IX_Mensagem_SalaFK",
                table: "mensagem",
                newName: "IX_mensagem_SalaFK");

            migrationBuilder.RenameIndex(
                name: "IX_Mensagem_RemetenteFK",
                table: "mensagem",
                newName: "IX_mensagem_RemetenteFK");

            migrationBuilder.AddColumn<int>(
                name: "Utilizador_RegistadoIDUtilizador",
                table: "Pedidos_Amizade",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_registados_Salas_Jogo",
                table: "registados_Salas_Jogo",
                columns: new[] { "UtilizadorFK", "SalaFK" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_registados_Salas_Chat",
                table: "registados_Salas_Chat",
                columns: new[] { "UtilizadorFK", "SalaFK" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pedidos_Amizade",
                table: "Pedidos_Amizade",
                column: "RemetenteFK");

            migrationBuilder.AddPrimaryKey(
                name: "PK_mensagem",
                table: "mensagem",
                columns: new[] { "IDMensagem", "SalaFK" });

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_Amizade_Utilizador_RegistadoIDUtilizador",
                table: "Pedidos_Amizade",
                column: "Utilizador_RegistadoIDUtilizador");

            migrationBuilder.AddForeignKey(
                name: "FK_mensagem_Salas_Chat_SalaFK",
                table: "mensagem",
                column: "SalaFK",
                principalTable: "Salas_Chat",
                principalColumn: "IDSala",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mensagem_Utilizador_Registado_RemetenteFK",
                table: "mensagem",
                column: "RemetenteFK",
                principalTable: "Utilizador_Registado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Amizade_Utilizador_Registado_Utilizador_RegistadoIDUtilizador",
                table: "Pedidos_Amizade",
                column: "Utilizador_RegistadoIDUtilizador",
                principalTable: "Utilizador_Registado",
                principalColumn: "IDUtilizador");

            migrationBuilder.AddForeignKey(
                name: "FK_registados_Salas_Chat_Salas_Chat_SalaFK",
                table: "registados_Salas_Chat",
                column: "SalaFK",
                principalTable: "Salas_Chat",
                principalColumn: "IDSala",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_registados_Salas_Chat_Utilizador_Registado_UtilizadorFK",
                table: "registados_Salas_Chat",
                column: "UtilizadorFK",
                principalTable: "Utilizador_Registado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_registados_Salas_Jogo_Sala_Jogo_1_Contra_1_SalaFK",
                table: "registados_Salas_Jogo",
                column: "SalaFK",
                principalTable: "Sala_Jogo_1_Contra_1",
                principalColumn: "IDSala",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_registados_Salas_Jogo_Utilizador_Registado_UtilizadorFK",
                table: "registados_Salas_Jogo",
                column: "UtilizadorFK",
                principalTable: "Utilizador_Registado",
                principalColumn: "IDUtilizador",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
