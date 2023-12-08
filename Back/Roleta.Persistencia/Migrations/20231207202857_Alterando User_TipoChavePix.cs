using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roleta.Persistencia.Migrations
{
    public partial class AlterandoUser_TipoChavePix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produto_pagamento",
                table: "Pagamentos");

            migrationBuilder.DropIndex(
                name: "IX_Pagamentos_ProdutoId",
                table: "Pagamentos");

            migrationBuilder.DropColumn(
                name: "ProdutoId",
                table: "Pagamentos");

            migrationBuilder.AddColumn<string>(
                name: "TipoChavePix",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoChavePix",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "ProdutoId",
                table: "Pagamentos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_ProdutoId",
                table: "Pagamentos",
                column: "ProdutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_pagamento",
                table: "Pagamentos",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
