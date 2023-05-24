using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkQR.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class VacationRejectionDescriptionMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Vacations",
                newName: "RequestDescription");

            migrationBuilder.AddColumn<bool>(
                name: "IsRejected",
                table: "Vacations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RejectionDescription",
                table: "Vacations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRejected",
                table: "Vacations");

            migrationBuilder.DropColumn(
                name: "RejectionDescription",
                table: "Vacations");

            migrationBuilder.RenameColumn(
                name: "RequestDescription",
                table: "Vacations",
                newName: "Description");
        }
    }
}
