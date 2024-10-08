using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIRST.Utilities.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActiveFtcMatches",
                columns: table => new
                {
                    FtcMatchId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveFtcMatches", x => x.FtcMatchId);
                    table.ForeignKey(
                        name: "FK_ActiveFtcMatches_FtcMatches_FtcMatchId",
                        column: x => x.FtcMatchId,
                        principalTable: "FtcMatches",
                        principalColumn: "FtcMatchId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveFtcMatches");
        }
    }
}
