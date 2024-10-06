using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIRST.Utilities.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSeriesToUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Match_Identification",
                table: "FtcMatches");

            migrationBuilder.CreateIndex(
                name: "IX_MatchNumber_TournamentLevel_Series",
                table: "FtcMatches",
                columns: new[] { "MatchNumber", "TournamentLevel", "Series" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MatchNumber_TournamentLevel_Series",
                table: "FtcMatches");

            migrationBuilder.CreateIndex(
                name: "Match_Identification",
                table: "FtcMatches",
                columns: new[] { "MatchNumber", "TournamentLevel" },
                unique: true);
        }
    }
}
