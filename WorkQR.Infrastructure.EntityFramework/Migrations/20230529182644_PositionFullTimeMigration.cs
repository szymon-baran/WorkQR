using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkQR.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class PositionFullTimeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFullTime",
                table: "Positions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFullTime",
                table: "Positions");
        }
    }
}
