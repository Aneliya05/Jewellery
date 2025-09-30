using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EllaJewelry.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "Email", "FirstName", "LastName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "UserName" },
                values: new object[] { "4601cb84-e21e-4962-8d27-9532dd555f8c", "localadmin@example.com", "Local", "Admin", "LOCALADMIN@EXAMPLE.COM", "LOCALADMIN", "AQAAAAIAAYagAAAAEJx4jOSVdt3fm0CsejG6PrqwH107ouSWs7g8M4diczuI98UhBtkp+EMjE2XYDup9Qg==", "+0000000000", "localadmin" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "Email", "FirstName", "LastName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "UserName" },
                values: new object[] { "b54fb5ad-a160-440b-bdb5-d88df64b1e78", "localowner@example.com", "Local", "Owner", "LOCALOWNER@EXAMPLE.COM", "LOCALOWNER", "AQAAAAIAAYagAAAAEMYsl/GfbOTufawQHGutwsGY1pIfXiMvH+T4bc0R+Frm37vQr/q/Se/muiypmKeOYQ==", "+0000000001", "localowner" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "Email", "FirstName", "LastName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "UserName" },
                values: new object[] { "99f22039-838f-4f18-87da-cb782c29e21f", "*****", "*****", "*****", "*****", "*****", "*****", "*****", "*****" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "Email", "FirstName", "LastName", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "UserName" },
                values: new object[] { "7295b807-302e-457c-9e24-1b41d1c57121", "*****", "*****", "*****", "*****", "*****", "*****", "*****", "*****" });
        }
    }
}
