using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mobu_backend.Migrations
{
    /// <inheritdoc />
    public partial class passwd_hex_val : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Utilizador_Registado",
                keyColumn: "ID_Utilizador",
                keyValue: 1,
                column: "passwHash",
                value: "E47E548A9EA2929625FCAD762E4BE370E03EC8F0E747446F1E0A3762C841A988F425DFE385DEF422B79460F8C293E02A");

            migrationBuilder.UpdateData(
                table: "Utilizador_Registado",
                keyColumn: "ID_Utilizador",
                keyValue: 2,
                column: "passwHash",
                value: "E47E548A9EA2929625FCAD762E4BE370E03EC8F0E747446F1E0A3762C841A988F425DFE385DEF422B79460F8C293E02A");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Utilizador_Registado",
                keyColumn: "ID_Utilizador",
                keyValue: 1,
                column: "passwHash",
                value: "5H5Uip6ikpYl/K12LkvjcOA+yPDnR0RvHgo3YshBqYj0Jd/jhd70IreUYPjCk+Aq");

            migrationBuilder.UpdateData(
                table: "Utilizador_Registado",
                keyColumn: "ID_Utilizador",
                keyValue: 2,
                column: "passwHash",
                value: "5H5Uip6ikpYl/K12LkvjcOA+yPDnR0RvHgo3YshBqYj0Jd/jhd70IreUYPjCk+Aq");
        }
    }
}
