using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIRST.Utilities.Migrations
{
    /// <inheritdoc />
    public partial class TeamsProgramCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Programs_ProgramId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_ProgramId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "Team_Identification",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "ProgramId",
                table: "Teams");

            migrationBuilder.AddColumn<string>(
                name: "ProgramCode",
                table: "Teams",
                type: "TEXT",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "Team_Identification",
                table: "Teams",
                columns: new[] { "TeamNumber", "SeasonYear", "ProgramCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Team_Identification",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "ProgramCode",
                table: "Teams");

            migrationBuilder.AddColumn<int>(
                name: "ProgramId",
                table: "Teams",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_ProgramId",
                table: "Teams",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "Team_Identification",
                table: "Teams",
                columns: new[] { "TeamNumber", "SeasonYear", "ProgramId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Programs_ProgramId",
                table: "Teams",
                column: "ProgramId",
                principalTable: "Programs",
                principalColumn: "ProgramId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
