using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zucchinimvc.Migrations
{
    /// <inheritdoc />
    public partial class FixUserLikedArticleKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLikedArticles",
                table: "UserLikedArticles");

            migrationBuilder.DropIndex(
                name: "IX_UserLikedArticles_ArticleId",
                table: "UserLikedArticles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserLikedArticles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLikedArticles",
                table: "UserLikedArticles",
                columns: new[] { "ArticleId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLikedArticles",
                table: "UserLikedArticles");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserLikedArticles",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLikedArticles",
                table: "UserLikedArticles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikedArticles_ArticleId",
                table: "UserLikedArticles",
                column: "ArticleId");
        }
    }
}
