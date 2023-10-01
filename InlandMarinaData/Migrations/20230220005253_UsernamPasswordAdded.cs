using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InlandMarinaData.Migrations
{
    public partial class UsernamPasswordAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Customer",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Customer",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Customer",
                keyColumn: "ID",
                keyValue: 1,
                columns: new[] { "Password", "Username" },
                values: new object[] { "password", "jdoe" });

            migrationBuilder.UpdateData(
                table: "Customer",
                keyColumn: "ID",
                keyValue: 2,
                columns: new[] { "Password", "Username" },
                values: new object[] { "password", "swilliams" });

            migrationBuilder.UpdateData(
                table: "Customer",
                keyColumn: "ID",
                keyValue: 3,
                columns: new[] { "Password", "Username" },
                values: new object[] { "password", "kwong" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Customer");
        }
    }
}
