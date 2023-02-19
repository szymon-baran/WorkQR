using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkQR.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddWorktimeEventsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorktimeEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    EventTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorktimeEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorktimeEvents_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorktimeEvents_ApplicationUserId",
                table: "WorktimeEvents",
                column: "ApplicationUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorktimeEvents");
        }
    }
}
