using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roleta.Persistencia.Migrations
{
    public partial class Add_DataCadastro_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("14e48948-e445-4a90-ba16-c8e6cbf5ca8e"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("4d9e9bbf-0ce6-4ec0-96b3-e4490f981244"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7fde3ce4-8f21-425a-b26c-0dd4cfd58b13"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCadastro",
                table: "AspNetUsers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("238e8ae4-3565-4282-ac78-3f4c46c035e5"), "ea7712c0-24b4-401b-b948-129256d9db46", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("57780b66-f464-4950-b35c-8515a8b1513b"), "f39ff057-9a48-4c38-84f4-80b4474bb2b9", "Afiliate", "AFILIATE" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("9cc9ed87-685c-4af1-a1d3-f359941553d2"), "a08d988d-d5f4-41ad-a561-8d6875a899fa", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("238e8ae4-3565-4282-ac78-3f4c46c035e5"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("57780b66-f464-4950-b35c-8515a8b1513b"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9cc9ed87-685c-4af1-a1d3-f359941553d2"));

            migrationBuilder.DropColumn(
                name: "DataCadastro",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("14e48948-e445-4a90-ba16-c8e6cbf5ca8e"), "3fd8f3e6-1d10-4cbe-99e3-d1fe0049458d", "Afiliate", "AFILIATE" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("4d9e9bbf-0ce6-4ec0-96b3-e4490f981244"), "35129dab-7945-48b3-8af7-d5ae8374c98a", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("7fde3ce4-8f21-425a-b26c-0dd4cfd58b13"), "25988f2b-9657-4c5d-bc3c-63a42a621c66", "Admin", "ADMIN" });
        }
    }
}
