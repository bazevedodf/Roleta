using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roleta.Persistencia.Migrations
{
    public partial class Inclusao_DemoAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<decimal>(
                name: "SaldoDeposito",
                table: "Produtos",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AddColumn<bool>(
                name: "DemoAcount",
                table: "AspNetUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("35a1de67-7411-402c-af95-f9a1f7e7562f"), "f4952cde-7e2d-4dd2-8a7c-aba5dd0d7732", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("8479520c-9baf-4c2b-8d83-019acb30701a"), "a5d106c1-eafe-42bf-8c87-d1a6d9585ccb", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("882f43a8-8c0c-4743-8066-edf47fc364eb"), "27c84ad3-7d52-459a-9159-4d8538f895a9", "Afiliate", "AFILIATE" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("35a1de67-7411-402c-af95-f9a1f7e7562f"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8479520c-9baf-4c2b-8d83-019acb30701a"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("882f43a8-8c0c-4743-8066-edf47fc364eb"));

            migrationBuilder.DropColumn(
                name: "DemoAcount",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<decimal>(
                name: "SaldoDeposito",
                table: "Produtos",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldDefaultValue: 0m);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("2f2dc261-e1e8-475a-88ba-ee501bbd0f73"), "dc8dc1ee-74f5-4204-ae99-f47669f0998f", "Apostador", "Apostador" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("56a286b7-3f5a-4cc1-875b-6b9735722eba"), "7446fabb-52ac-4237-9b43-a891c2a568b6", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("7e0e18e6-423b-4e97-bf8e-6d30bff36638"), "05542b26-9f88-445c-b342-abc543e5f4a6", "Afiliado", "Afiliado" });
        }
    }
}
