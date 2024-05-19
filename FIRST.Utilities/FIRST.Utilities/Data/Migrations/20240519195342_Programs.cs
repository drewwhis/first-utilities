using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIRST.Utilities.Migrations
{
    /// <inheritdoc />
    public partial class Programs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    ProgramId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProgramCode = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    ActiveSeasonYear = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.ProgramId);
                });

            migrationBuilder.InsertData(
                table: "Programs",
                columns: new[] { "ProgramId", "ActiveSeasonYear", "ProgramCode" },
                values: new object[] { 1, 2023, "FTC" });

            migrationBuilder.CreateIndex(
                name: "IX_Programs_ProgramCode",
                table: "Programs",
                column: "ProgramCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Programs");
        }
    }
}
