using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeliculasApiRestFul.Migrations
{
    /// <inheritdoc />
    public partial class AdminData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "aa5c881a-8bff-4cf3-9b53-1c638954ce98", null, "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "fa686ac7-2580-4262-81b2-29ffb20d4a9d", 0, "d969b07d-99d9-423a-a246-21ca9cd73764", "wilmer994@gmail.com", false, false, null, "wilmer994@gmail.com", "wilmer994@gmail.com", "AQAAAAIAAYagAAAAEB5hjf0w1uftan41cZwXNbPOyPSAOZLLawXYB/ggf/YYw+9sgFej4kYgtslsXq7Igg==", null, false, "802a3efa-7d6b-4eb2-8a02-001fc5f0a709", false, "wilmer994@gmail.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "UserId" },
                values: new object[] { 1, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin", "fa686ac7-2580-4262-81b2-29ffb20d4a9d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aa5c881a-8bff-4cf3-9b53-1c638954ce98");

            migrationBuilder.DeleteData(
                table: "AspNetUserClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fa686ac7-2580-4262-81b2-29ffb20d4a9d");
        }
    }
}
