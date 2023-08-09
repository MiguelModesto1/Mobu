using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class identity_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthenticationID",
                table: "Utilizador_Registado",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthenticationID",
                table: "Utilizador_Anonimo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthenticationID",
                table: "Admin",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthenticationID",
                table: "Utilizador_Registado");

            migrationBuilder.DropColumn(
                name: "AuthenticationID",
                table: "Utilizador_Anonimo");

            migrationBuilder.DropColumn(
                name: "AuthenticationID",
                table: "Admin");
        }
    }
}
