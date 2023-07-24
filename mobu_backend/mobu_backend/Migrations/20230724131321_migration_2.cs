using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class migration_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Mensagem",
                table: "Mensagem");

            migrationBuilder.AddColumn<int>(
                name: "IDMensagemSala",
                table: "Mensagem",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mensagem",
                table: "Mensagem",
                column: "IDMensagemSala");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Mensagem",
                table: "Mensagem");

            migrationBuilder.DropColumn(
                name: "IDMensagemSala",
                table: "Mensagem");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mensagem",
                table: "Mensagem",
                columns: new[] { "IDMensagem", "SalaFK" });
        }
    }
}
