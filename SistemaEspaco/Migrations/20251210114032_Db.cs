using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEspaco.Migrations
{
    /// <inheritdoc />
    public partial class Db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Espaco",
                columns: table => new
                {
                    IdEspaco = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Capacidade = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Espaco__1EDE22F2CFB9AFFA", x => x.IdEspaco);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Telefone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuario__5B65BF979A7A72BD", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Reserva",
                columns: table => new
                {
                    IdReserva = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<int>(type: "int", nullable: true),
                    IdEspaco = table.Column<int>(type: "int", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime", nullable: false),
                    Situacao = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true, defaultValue: "Pendente")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reserva__0E49C69D01877F98", x => x.IdReserva);
                    table.ForeignKey(
                        name: "FK__Reserva__IdEspac__4F7CD00D",
                        column: x => x.IdEspaco,
                        principalTable: "Espaco",
                        principalColumn: "IdEspaco");
                    table.ForeignKey(
                        name: "FK__Reserva__IdUsuar__4E88ABD4",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_IdEspaco",
                table: "Reserva",
                column: "IdEspaco");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_IdUsuario",
                table: "Reserva",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reserva");

            migrationBuilder.DropTable(
                name: "Espaco");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
