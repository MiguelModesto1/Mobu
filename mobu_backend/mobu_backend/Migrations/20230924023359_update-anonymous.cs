using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class updateanonymous : Migration
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
