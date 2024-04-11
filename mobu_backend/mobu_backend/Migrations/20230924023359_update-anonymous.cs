using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    public partial class updateanonymous : Migration
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnderecoIPv4",
                table: "Utilizador_Anonimo");

            migrationBuilder.DropColumn(
                name: "EnderecoIPv6",
                table: "Utilizador_Anonimo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EnderecoIPv4",
                table: "Utilizador_Anonimo",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnderecoIPv6",
                table: "Utilizador_Anonimo",
                type: "nvarchar(39)",
                maxLength: 39,
                nullable: true);
        }
    }
}
