using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roleta.Persistencia.Migrations
{
    public partial class AlterandoTransacaoRoleta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransacoesRoleta_Transacoes_TransacaoId",
                table: "TransacoesRoleta");

            migrationBuilder.DropIndex(
                name: "IX_TransacoesRoleta_TransacaoId",
                table: "TransacoesRoleta");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "TransacoesRoleta");

            migrationBuilder.RenameColumn(
                name: "SaldoLucro",
                table: "Roletas",
                newName: "ValorSaldo");

            migrationBuilder.RenameColumn(
                name: "SaldoBanca",
                table: "Roletas",
                newName: "ValorBanca");
        }
    }
}
