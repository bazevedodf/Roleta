using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Roleta.Persistencia.Migrations
{
    public partial class Inclusao_AfiliateCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "AfiliateCode",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "AfiliateCode",
                table: "AspNetUsers");

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
    }
}
