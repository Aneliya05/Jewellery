using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EllaJewelry.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b4626d44-0f16-4797-be6d-8e23e2fb5e7b", "AQAAAAIAAYagAAAAEAQWTvHJclPYmsCkv1eLpFmfVB7HdYQGSGJy5cGqV4546HLK3vE7sHjTv6BHRZ+Izg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "f9e1ae82-6f1b-415b-8148-e2f43f04d9f2", "AQAAAAIAAYagAAAAEAE3JWmzD6rxt7CDofMCcaPJGcRrlPHkpcSL+uA/N9uMTYjeRjMUIj5vCa/m/TaKGQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4601cb84-e21e-4962-8d27-9532dd555f8c", "AQAAAAIAAYagAAAAEJx4jOSVdt3fm0CsejG6PrqwH107ouSWs7g8M4diczuI98UhBtkp+EMjE2XYDup9Qg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b54fb5ad-a160-440b-bdb5-d88df64b1e78", "AQAAAAIAAYagAAAAEMYsl/GfbOTufawQHGutwsGY1pIfXiMvH+T4bc0R+Frm37vQr/q/Se/muiypmKeOYQ==" });
        }
    }
}
