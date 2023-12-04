using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roleta.Persistencia.Migrations
{
    public partial class IncluindoPercentialdecomissao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Comissao",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comissao",
                table: "AspNetUsers");
        }
    }
}
