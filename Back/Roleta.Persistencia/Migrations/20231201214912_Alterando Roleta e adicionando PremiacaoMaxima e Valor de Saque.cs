using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roleta.Persistencia.Migrations
{
    public partial class AlterandoRoletaeadicionandoPremiacaoMaximaeValordeSaque : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FreeSpin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SaldoDeposito",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SaldoSaque",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "PremiacaoMaxima",
                table: "Roletas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ValorSaque",
                table: "Roletas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Roletas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PremiacaoMaxima", "ValorSaque" },
                values: new object[] { 10, 50 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PremiacaoMaxima",
                table: "Roletas");

            migrationBuilder.DropColumn(
                name: "ValorSaque",
                table: "Roletas");

            migrationBuilder.AddColumn<int>(
                name: "FreeSpin",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
