using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roleta.Persistencia.Migrations
{
    public partial class AlterandoCarteiraadicionadoSaldoDemo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransacoesRoleta_Roletas_RoletaSorteId",
                table: "TransacoesRoleta");

            migrationBuilder.AlterColumn<int>(
                name: "RoletaSorteId",
                table: "TransacoesRoleta",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "SaldoDemo",
                table: "Carteiras",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_TransacoesRoleta_Roletas_RoletaSorteId",
                table: "TransacoesRoleta",
                column: "RoletaSorteId",
                principalTable: "Roletas",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransacoesRoleta_Roletas_RoletaSorteId",
                table: "TransacoesRoleta");

            migrationBuilder.DropColumn(
                name: "SaldoDemo",
                table: "Carteiras");

            migrationBuilder.AlterColumn<int>(
                name: "RoletaSorteId",
                table: "TransacoesRoleta",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TransacoesRoleta_Roletas_RoletaSorteId",
                table: "TransacoesRoleta",
                column: "RoletaSorteId",
                principalTable: "Roletas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
