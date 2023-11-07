using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roleta.Persistencia.Migrations
{
    public partial class AdicionandosaldoDepositoemProduto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9059e131-db4e-48ec-b8fa-9a03318ce294"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("929290a4-9f69-4571-a87b-57285646d3cd"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9ceee7ad-0e67-4634-939b-a06b58cf6c52"));

            migrationBuilder.AddColumn<decimal>(
                name: "SaldoDeposito",
                table: "Produtos",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("2f2dc261-e1e8-475a-88ba-ee501bbd0f73"), "dc8dc1ee-74f5-4204-ae99-f47669f0998f", "Apostador", "Apostador" },
                    { new Guid("56a286b7-3f5a-4cc1-875b-6b9735722eba"), "7446fabb-52ac-4237-9b43-a891c2a568b6", "Admin", "ADMIN" },
                    { new Guid("7e0e18e6-423b-4e97-bf8e-6d30bff36638"), "05542b26-9f88-445c-b342-abc543e5f4a6", "Afiliado", "Afiliado" }
                });

            migrationBuilder.UpdateData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 1,
                column: "SaldoDeposito",
                value: 75m);

            migrationBuilder.UpdateData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 2,
                column: "SaldoDeposito",
                value: 200m);

            migrationBuilder.UpdateData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: 3,
                column: "SaldoDeposito",
                value: 500m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2f2dc261-e1e8-475a-88ba-ee501bbd0f73"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("56a286b7-3f5a-4cc1-875b-6b9735722eba"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7e0e18e6-423b-4e97-bf8e-6d30bff36638"));

            migrationBuilder.DropColumn(
                name: "SaldoDeposito",
                table: "Produtos");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("9059e131-db4e-48ec-b8fa-9a03318ce294"), "43425a34-aff8-4e5d-8148-4f9f7e90d740", "Afiliado", "Afiliado" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("929290a4-9f69-4571-a87b-57285646d3cd"), "8e3e47e3-4c9d-4494-978e-051636359c13", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("9ceee7ad-0e67-4634-939b-a06b58cf6c52"), "b1a3c2a1-656e-48aa-9ab5-78354439dea6", "Apostador", "Apostador" });
        }
    }
}
