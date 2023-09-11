using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieFInderServer.Migrations
{
    /// <inheritdoc />
    public partial class Users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "SavedMovieUser",
                columns: table => new
                {
                    LikedByUsersUserId = table.Column<int>(type: "int", nullable: false),
                    LikedMoviesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedMovieUser", x => new { x.LikedByUsersUserId, x.LikedMoviesId });
                    table.ForeignKey(
                        name: "FK_SavedMovieUser_SavedMovies_LikedMoviesId",
                        column: x => x.LikedMoviesId,
                        principalTable: "SavedMovies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedMovieUser_Users_LikedByUsersUserId",
                        column: x => x.LikedByUsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavedMovieUser_LikedMoviesId",
                table: "SavedMovieUser",
                column: "LikedMoviesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedMovieUser");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
