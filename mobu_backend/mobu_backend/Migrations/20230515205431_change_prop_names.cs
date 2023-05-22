using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class change_prop_names : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_registados_Salas_Jogo_Sala_Jogo_1_Contra_1_Sala_Jogo_1_Contra_1ID_Sala",
                table: "registados_Salas_Jogo");

            migrationBuilder.RenameColumn(
                name: "ID_Utilizador",
                table: "Utilizador_Registado",
                newName: "IDUtilizador");

            migrationBuilder.RenameColumn(
                name: "ID_Utilizador",
                table: "Utilizador_Anonimo",
                newName: "IDUtilizador");

            migrationBuilder.RenameColumn(
                name: "Se_grupo",
                table: "Salas_Chat",
                newName: "SeGrupo");

            migrationBuilder.RenameColumn(
                name: "Nome_sala",
                table: "Salas_Chat",
                newName: "NomeSala");

            migrationBuilder.RenameColumn(
                name: "ID_Sala",
                table: "Salas_Chat",
                newName: "IDSala");

            migrationBuilder.RenameColumn(
                name: "ID_Sala",
                table: "Sala_Jogo_1_Contra_1",
                newName: "IDSala");

            migrationBuilder.RenameColumn(
                name: "Sala_Jogo_1_Contra_1ID_Sala",
                table: "registados_Salas_Jogo",
                newName: "Sala_Jogo_1_Contra_1IDSala");

            migrationBuilder.RenameColumn(
                name: "Is_fundador",
                table: "registados_Salas_Jogo",
                newName: "IsFundador");

            migrationBuilder.RenameIndex(
                name: "IX_registados_Salas_Jogo_Sala_Jogo_1_Contra_1ID_Sala",
                table: "registados_Salas_Jogo",
                newName: "IX_registados_Salas_Jogo_Sala_Jogo_1_Contra_1IDSala");

            migrationBuilder.RenameColumn(
                name: "Estado_pedido",
                table: "Pedidos_Amizade",
                newName: "EstadoPedido");

            migrationBuilder.RenameColumn(
                name: "Estado_Mensagem",
                table: "mensagem",
                newName: "EstadoMensagem");

            migrationBuilder.RenameColumn(
                name: "Conteudo_Msg",
                table: "mensagem",
                newName: "ConteudoMsg");

            migrationBuilder.RenameColumn(
                name: "ID_Mensagem",
                table: "mensagem",
                newName: "IDMensagem");

            migrationBuilder.AddForeignKey(
                name: "FK_registados_Salas_Jogo_Sala_Jogo_1_Contra_1_Sala_Jogo_1_Contra_1IDSala",
                table: "registados_Salas_Jogo",
                column: "Sala_Jogo_1_Contra_1IDSala",
                principalTable: "Sala_Jogo_1_Contra_1",
                principalColumn: "IDSala");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_registados_Salas_Jogo_Sala_Jogo_1_Contra_1_Sala_Jogo_1_Contra_1IDSala",
                table: "registados_Salas_Jogo");

            migrationBuilder.RenameColumn(
                name: "IDUtilizador",
                table: "Utilizador_Registado",
                newName: "ID_Utilizador");

            migrationBuilder.RenameColumn(
                name: "IDUtilizador",
                table: "Utilizador_Anonimo",
                newName: "ID_Utilizador");

            migrationBuilder.RenameColumn(
                name: "SeGrupo",
                table: "Salas_Chat",
                newName: "Se_grupo");

            migrationBuilder.RenameColumn(
                name: "NomeSala",
                table: "Salas_Chat",
                newName: "Nome_sala");

            migrationBuilder.RenameColumn(
                name: "IDSala",
                table: "Salas_Chat",
                newName: "ID_Sala");

            migrationBuilder.RenameColumn(
                name: "IDSala",
                table: "Sala_Jogo_1_Contra_1",
                newName: "ID_Sala");

            migrationBuilder.RenameColumn(
                name: "Sala_Jogo_1_Contra_1IDSala",
                table: "registados_Salas_Jogo",
                newName: "Sala_Jogo_1_Contra_1ID_Sala");

            migrationBuilder.RenameColumn(
                name: "IsFundador",
                table: "registados_Salas_Jogo",
                newName: "Is_fundador");

            migrationBuilder.RenameIndex(
                name: "IX_registados_Salas_Jogo_Sala_Jogo_1_Contra_1IDSala",
                table: "registados_Salas_Jogo",
                newName: "IX_registados_Salas_Jogo_Sala_Jogo_1_Contra_1ID_Sala");

            migrationBuilder.RenameColumn(
                name: "EstadoPedido",
                table: "Pedidos_Amizade",
                newName: "Estado_pedido");

            migrationBuilder.RenameColumn(
                name: "EstadoMensagem",
                table: "mensagem",
                newName: "Estado_Mensagem");

            migrationBuilder.RenameColumn(
                name: "ConteudoMsg",
                table: "mensagem",
                newName: "Conteudo_Msg");

            migrationBuilder.RenameColumn(
                name: "IDMensagem",
                table: "mensagem",
                newName: "ID_Mensagem");

            migrationBuilder.AddForeignKey(
                name: "FK_registados_Salas_Jogo_Sala_Jogo_1_Contra_1_Sala_Jogo_1_Contra_1ID_Sala",
                table: "registados_Salas_Jogo",
                column: "Sala_Jogo_1_Contra_1ID_Sala",
                principalTable: "Sala_Jogo_1_Contra_1",
                principalColumn: "ID_Sala");
        }
    }
}
