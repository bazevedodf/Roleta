using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roleta.Persistencia.Migrations
{
    public partial class Include_BancasPagadoras : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Comissao",
                table: "AspNetUsers",
                newName: "ComissaoPercentual");

            migrationBuilder.AddColumn<decimal>(
                name: "ComissaoFixa",
                table: "AspNetUsers",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "BancasPagadora",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SaldoDia = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    DataBanca = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RoletaSorteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BancasPagadora", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BancasPagadora_Roletas_RoletaSorteId",
                        column: x => x.RoletaSorteId,
                        principalTable: "Roletas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BancasPagadora_RoletaSorteId",
                table: "BancasPagadora",
                column: "RoletaSorteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BancasPagadora");

            migrationBuilder.DropColumn(
                name: "ComissaoFixa",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ComissaoPercentual",
                table: "AspNetUsers",
                newName: "Comissao");

        }
    }
}
