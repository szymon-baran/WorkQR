using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkQR.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class UserRoleNameInPositionsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserRoleName",
                table: "Positions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRoleName",
                table: "Positions");
        }
    }
}
