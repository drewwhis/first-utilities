using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIRST.Utilities.Migrations
{
    /// <inheritdoc />
    public partial class ActiveEventCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ActiveEvents_EventId",
                table: "ActiveEvents");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "ActiveEvents",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveEvents_EventId",
                table: "ActiveEvents",
                column: "EventId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ActiveEvents_EventId",
                table: "ActiveEvents");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "ActiveEvents",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActiveEvents_EventId",
                table: "ActiveEvents",
                column: "EventId");
        }
    }
}
