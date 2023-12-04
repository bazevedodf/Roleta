using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roleta.Persistencia.Migrations
{
    public partial class IncluidoRoletaeTransacaoRoleta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roletas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValorBanca = table.Column<decimal>(type: "decimal(65,30)", nullable: false, defaultValue: 0m),
                    ValorSaldo = table.Column<decimal>(type: "decimal(65,30)", nullable: false, defaultValue: 0m),
                    PercentualBanca = table.Column<int>(type: "int", nullable: false, defaultValue: 60)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roletas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TransacoesRoleta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    valor = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "NOW()"),
                    TransacaoId = table.Column<int>(type: "int", maxLength: 15, nullable: false),
                    RoletaId = table.Column<int>(type: "int", nullable: false),
                    RoletaSorteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransacoesRoleta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransacoesRoleta_Roletas_RoletaSorteId",
                        column: x => x.RoletaSorteId,
                        principalTable: "Roletas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Roletas",
                columns: new[] { "Id", "Nome", "PercentualBanca" },
                values: new object[] { 1, "RoletaSorte", 60 });

            migrationBuilder.CreateIndex(
                name: "IX_TransacoesRoleta_RoletaSorteId",
                table: "TransacoesRoleta",
                column: "RoletaSorteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransacoesRoleta");

            migrationBuilder.DropTable(
                name: "Roletas");
        }
    }
}
