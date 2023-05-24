using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkQR.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRegistrationCodeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RegistrationCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationCode",
                table: "AspNetUsers");
        }
    }
}
