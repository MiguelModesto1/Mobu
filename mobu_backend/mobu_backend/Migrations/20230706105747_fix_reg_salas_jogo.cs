using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class fix_reg_salas_jogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_registados_Salas_Jogo_Sala_Jogo_1_Contra_1_Sala_Jogo_1_Contra_1IDSala",
                table: "registados_Salas_Jogo");

            migrationBuilder.DropForeignKey(
                name: "FK_registados_Salas_Jogo_Salas_Chat_SalaFK",
                table: "registados_Salas_Jogo");

            migrationBuilder.DropIndex(
                name: "IX_registados_Salas_Jogo_Sala_Jogo_1_Contra_1IDSala",
                table: "registados_Salas_Jogo");

            migrationBuilder.DropColumn(
                name: "Sala_Jogo_1_Contra_1IDSala",
                table: "registados_Salas_Jogo");

            migrationBuilder.AddForeignKey(
                name: "FK_registados_Salas_Jogo_Sala_Jogo_1_Contra_1_SalaFK",
                table: "registados_Salas_Jogo",
                column: "SalaFK",
                principalTable: "Sala_Jogo_1_Contra_1",
                principalColumn: "IDSala",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_registados_Salas_Jogo_Sala_Jogo_1_Contra_1_SalaFK",
                table: "registados_Salas_Jogo");

            migrationBuilder.AddColumn<int>(
                name: "Sala_Jogo_1_Contra_1IDSala",
                table: "registados_Salas_Jogo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_registados_Salas_Jogo_Sala_Jogo_1_Contra_1IDSala",
                table: "registados_Salas_Jogo",
                column: "Sala_Jogo_1_Contra_1IDSala");

            migrationBuilder.AddForeignKey(
                name: "FK_registados_Salas_Jogo_Sala_Jogo_1_Contra_1_Sala_Jogo_1_Contra_1IDSala",
                table: "registados_Salas_Jogo",
                column: "Sala_Jogo_1_Contra_1IDSala",
                principalTable: "Sala_Jogo_1_Contra_1",
                principalColumn: "IDSala");

            migrationBuilder.AddForeignKey(
                name: "FK_registados_Salas_Jogo_Salas_Chat_SalaFK",
                table: "registados_Salas_Jogo",
                column: "SalaFK",
                principalTable: "Salas_Chat",
                principalColumn: "IDSala",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
