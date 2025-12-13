using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEspaco.Migrations
{
    /// <inheritdoc />
    public partial class InitialProjetoEspaco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DashboardReservaViewModel",
                columns: table => new
                {
                    TotalReservas = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservasAtivas = table.Column<int>(type: "int", nullable: false),
                    ReservasFinalizadas = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardReservaViewModel", x => x.TotalReservas);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DashboardReservaViewModel");
        }
    }
}
