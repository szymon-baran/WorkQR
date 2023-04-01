using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkQR.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemPositionTypeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSystemPosition",
                table: "Positions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSystemPosition",
                table: "Positions");
        }
    }
}
