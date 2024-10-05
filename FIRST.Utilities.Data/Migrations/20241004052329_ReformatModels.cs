using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIRST.Utilities.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReformatModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveEvents");

            migrationBuilder.DropTable(
                name: "EventTeams");

            migrationBuilder.DropTable(
                name: "MatchTeam");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropTable(
                name: "Match");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.CreateTable(
                name: "ActiveProgramSeasons",
                columns: table => new
                {
                    ActiveProgramSeasonId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProgramCode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    SeasonYear = table.Column<int>(type: "INTEGER", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveProgramSeasons", x => x.ActiveProgramSeasonId);
                });

            migrationBuilder.CreateTable(
                name: "FtcEvents",
                columns: table => new
                {
                    FtcEventId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    EventCode = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    SeasonYear = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FtcEvents", x => x.FtcEventId);
                });

            migrationBuilder.CreateTable(
                name: "ActiveFtcEvents",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveFtcEvents", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_ActiveFtcEvents_FtcEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "FtcEvents",
                        principalColumn: "FtcEventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FtcMatches",
                columns: table => new
                {
                    FtcMatchId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MatchNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Field = table.Column<string>(type: "TEXT", nullable: true),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Series = table.Column<int>(type: "INTEGER", nullable: false),
                    TournamentLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FtcMatches", x => x.FtcMatchId);
                    table.ForeignKey(
                        name: "FK_FtcMatches_FtcEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "FtcEvents",
                        principalColumn: "FtcEventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FtcTeams",
                columns: table => new
                {
                    FtcTeamId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TeamNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    ShortName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    SeasonYear = table.Column<int>(type: "INTEGER", nullable: false),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FtcTeams", x => x.FtcTeamId);
                    table.ForeignKey(
                        name: "FK_FtcTeams_FtcEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "FtcEvents",
                        principalColumn: "FtcEventId",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "Program_Identification",
                table: "ActiveProgramSeasons",
                column: "ProgramCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Event_Identification",
                table: "FtcEvents",
                columns: new[] { "EventCode", "SeasonYear" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Match_Identification",
                table: "FtcMatches",
                columns: new[] { "EventId", "MatchNumber", "TournamentLevel" },
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

            migrationBuilder.CreateIndex(
                name: "IX_FtcTeams_EventId",
                table: "FtcTeams",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "Team_Identification",
                table: "FtcTeams",
                columns: new[] { "TeamNumber", "SeasonYear" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveFtcEvents");

            migrationBuilder.DropTable(
                name: "ActiveProgramSeasons");

            migrationBuilder.DropTable(
                name: "FtcTeamMatchJoin");

            migrationBuilder.DropTable(
                name: "FtcMatches");

            migrationBuilder.DropTable(
                name: "FtcTeams");

            migrationBuilder.DropTable(
                name: "FtcEvents");

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventCode = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    EventName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    ProgramCode = table.Column<string>(type: "TEXT", maxLength: 80, nullable: false),
                    SeasonYear = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    ProgramId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActiveSeasonYear = table.Column<int>(type: "INTEGER", nullable: false),
                    ProgramCode = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.ProgramId);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    ProgramCode = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    SeasonYear = table.Column<int>(type: "INTEGER", nullable: false),
                    ShortName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    TeamNumber = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.TeamId);
                });

            migrationBuilder.CreateTable(
                name: "ActiveEvents",
                columns: table => new
                {
                    ActiveEventId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveEvents", x => x.ActiveEventId);
                    table.ForeignKey(
                        name: "FK_ActiveEvents_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    MatchId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    Field = table.Column<string>(type: "TEXT", nullable: true),
                    MatchNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Series = table.Column<int>(type: "INTEGER", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.MatchId);
                    table.ForeignKey(
                        name: "FK_Match_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventTeams",
                columns: table => new
                {
                    EventTeamId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTeams", x => x.EventTeamId);
                    table.ForeignKey(
                        name: "FK_EventTeams_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchTeam",
                columns: table => new
                {
                    MatchTeamId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MatchId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false),
                    Station = table.Column<string>(type: "TEXT", maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchTeam", x => x.MatchTeamId);
                    table.ForeignKey(
                        name: "FK_MatchTeam_Match_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Match",
                        principalColumn: "MatchId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchTeam_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Programs",
                columns: new[] { "ProgramId", "ActiveSeasonYear", "ProgramCode" },
                values: new object[] { 1, 2023, "FTC" });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveEvents_EventId",
                table: "ActiveEvents",
                column: "EventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Event_Identification",
                table: "Events",
                columns: new[] { "ProgramCode", "EventCode", "SeasonYear" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventTeams_TeamId",
                table: "EventTeams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "Team_Registration",
                table: "EventTeams",
                columns: new[] { "EventId", "TeamId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Match_EventId",
                table: "Match",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchTeam_MatchId",
                table: "MatchTeam",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchTeam_TeamId",
                table: "MatchTeam",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_ProgramCode",
                table: "Programs",
                column: "ProgramCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Team_Identification",
                table: "Teams",
                columns: new[] { "TeamNumber", "SeasonYear", "ProgramCode" },
                unique: true);
        }
    }
}
