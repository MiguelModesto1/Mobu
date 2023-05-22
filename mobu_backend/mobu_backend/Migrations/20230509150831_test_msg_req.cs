using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class test_msg_req : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mensagem_Salas_Chat_salaFK",
                table: "mensagem");

            migrationBuilder.DropForeignKey(
                name: "FK_mensagem_Utilizador_Registado_remetenteFK",
                table: "mensagem");

            migrationBuilder.RenameColumn(
                name: "passwHash",
                table: "Utilizador_Registado",
                newName: "PasswHash");

            migrationBuilder.RenameColumn(
                name: "remetenteFK",
                table: "mensagem",
                newName: "RemetenteFK");

            migrationBuilder.RenameColumn(
                name: "salaFK",
                table: "mensagem",
                newName: "SalaFK");

            migrationBuilder.RenameIndex(
                name: "IX_mensagem_salaFK",
                table: "mensagem",
                newName: "IX_mensagem_SalaFK");

            migrationBuilder.RenameIndex(
                name: "IX_mensagem_remetenteFK",
                table: "mensagem",
                newName: "IX_mensagem_RemetenteFK");

            migrationBuilder.AlterColumn<string>(
                name: "Nome_sala",
                table: "Salas_Chat",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddForeignKey(
                name: "FK_mensagem_Salas_Chat_SalaFK",
                table: "mensagem",
                column: "SalaFK",
                principalTable: "Salas_Chat",
                principalColumn: "ID_Sala",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mensagem_Utilizador_Registado_RemetenteFK",
                table: "mensagem",
                column: "RemetenteFK",
                principalTable: "Utilizador_Registado",
                principalColumn: "ID_Utilizador",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mensagem_Salas_Chat_SalaFK",
                table: "mensagem");

            migrationBuilder.DropForeignKey(
                name: "FK_mensagem_Utilizador_Registado_RemetenteFK",
                table: "mensagem");

            migrationBuilder.RenameColumn(
                name: "PasswHash",
                table: "Utilizador_Registado",
                newName: "passwHash");

            migrationBuilder.RenameColumn(
                name: "RemetenteFK",
                table: "mensagem",
                newName: "remetenteFK");

            migrationBuilder.RenameColumn(
                name: "SalaFK",
                table: "mensagem",
                newName: "salaFK");

            migrationBuilder.RenameIndex(
                name: "IX_mensagem_SalaFK",
                table: "mensagem",
                newName: "IX_mensagem_salaFK");

            migrationBuilder.RenameIndex(
                name: "IX_mensagem_RemetenteFK",
                table: "mensagem",
                newName: "IX_mensagem_remetenteFK");

            migrationBuilder.AlterColumn<string>(
                name: "Nome_sala",
                table: "Salas_Chat",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_mensagem_Salas_Chat_salaFK",
                table: "mensagem",
                column: "salaFK",
                principalTable: "Salas_Chat",
                principalColumn: "ID_Sala",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mensagem_Utilizador_Registado_remetenteFK",
                table: "mensagem",
                column: "remetenteFK",
                principalTable: "Utilizador_Registado",
                principalColumn: "ID_Utilizador",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
