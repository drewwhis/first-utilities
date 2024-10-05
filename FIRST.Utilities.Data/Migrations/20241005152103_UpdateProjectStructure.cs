using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIRST.Utilities.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FtcMatches_FtcEvents_EventId",
                table: "FtcMatches");

            migrationBuilder.DropForeignKey(
                name: "FK_FtcTeams_FtcEvents_EventId",
                table: "FtcTeams");

            migrationBuilder.DropTable(
                name: "FtcTeamMatchJoin");

            migrationBuilder.DropIndex(
                name: "IX_FtcTeams_EventId",
                table: "FtcTeams");

            migrationBuilder.DropIndex(
                name: "Team_Identification",
                table: "FtcTeams");

            migrationBuilder.DropIndex(
                name: "Match_Identification",
                table: "FtcMatches");

            migrationBuilder.DropIndex(
                name: "Event_Identification",
                table: "FtcEvents");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "FtcTeams");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "FtcMatches");

            migrationBuilder.RenameIndex(
                name: "Program_Identification",
                table: "ActiveProgramSeasons",
                newName: "ProgramCode");

            migrationBuilder.AddColumn<int>(
                name: "FtcTeamId",
                table: "FtcMatches",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "TeamNumber",
                table: "FtcTeams",
                column: "TeamNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FtcMatches_FtcTeamId",
                table: "FtcMatches",
                column: "FtcTeamId");

            migrationBuilder.CreateIndex(
                name: "Match_Identification",
                table: "FtcMatches",
                columns: new[] { "MatchNumber", "TournamentLevel" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EventCode",
                table: "FtcEvents",
                column: "EventCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FtcMatches_FtcTeams_FtcTeamId",
                table: "FtcMatches",
                column: "FtcTeamId",
                principalTable: "FtcTeams",
                principalColumn: "FtcTeamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FtcMatches_FtcTeams_FtcTeamId",
                table: "FtcMatches");

            migrationBuilder.DropIndex(
                name: "TeamNumber",
                table: "FtcTeams");

            migrationBuilder.DropIndex(
                name: "IX_FtcMatches_FtcTeamId",
                table: "FtcMatches");

            migrationBuilder.DropIndex(
                name: "Match_Identification",
                table: "FtcMatches");

            migrationBuilder.DropIndex(
                name: "EventCode",
                table: "FtcEvents");

            migrationBuilder.DropColumn(
                name: "FtcTeamId",
                table: "FtcMatches");

            migrationBuilder.RenameIndex(
                name: "ProgramCode",
                table: "ActiveProgramSeasons",
                newName: "Program_Identification");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "FtcTeams",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "FtcMatches",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FtcTeamMatchJoin",
                columns: table => new
                {
                    MatchesFtcMatchId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamsFtcTeamId = table.Column<int>(type: "INTEGER", nullable: false),
                    MatchFtcMatchId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamFtcTeamId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FtcTeamMatchJoin", x => new { x.MatchesFtcMatchId, x.TeamsFtcTeamId });
                    table.ForeignKey(
                        name: "FK_FtcTeamMatchJoin_FtcMatches_MatchFtcMatchId",
                        column: x => x.MatchFtcMatchId,
                        principalTable: "FtcMatches",
                        principalColumn: "FtcMatchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FtcTeamMatchJoin_FtcMatches_MatchesFtcMatchId",
                        column: x => x.MatchesFtcMatchId,
                        principalTable: "FtcMatches",
                        principalColumn: "FtcMatchId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FtcTeamMatchJoin_FtcTeams_TeamFtcTeamId",
                        column: x => x.TeamFtcTeamId,
                        principalTable: "FtcTeams",
                        principalColumn: "FtcTeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FtcTeamMatchJoin_FtcTeams_TeamsFtcTeamId",
                        column: x => x.TeamsFtcTeamId,
                        principalTable: "FtcTeams",
                        principalColumn: "FtcTeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FtcTeams_EventId",
                table: "FtcTeams",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "Team_Identification",
                table: "FtcTeams",
                columns: new[] { "TeamNumber", "SeasonYear" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Match_Identification",
                table: "FtcMatches",
                columns: new[] { "EventId", "MatchNumber", "TournamentLevel" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Event_Identification",
                table: "FtcEvents",
                columns: new[] { "EventCode", "SeasonYear" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FtcTeamMatchJoin_MatchFtcMatchId",
                table: "FtcTeamMatchJoin",
                column: "MatchFtcMatchId");

            migrationBuilder.CreateIndex(
                name: "IX_FtcTeamMatchJoin_TeamFtcTeamId",
                table: "FtcTeamMatchJoin",
                column: "TeamFtcTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_FtcTeamMatchJoin_TeamsFtcTeamId",
                table: "FtcTeamMatchJoin",
                column: "TeamsFtcTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_FtcMatches_FtcEvents_EventId",
                table: "FtcMatches",
                column: "EventId",
                principalTable: "FtcEvents",
                principalColumn: "FtcEventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FtcTeams_FtcEvents_EventId",
                table: "FtcTeams",
                column: "EventId",
                principalTable: "FtcEvents",
                principalColumn: "FtcEventId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
