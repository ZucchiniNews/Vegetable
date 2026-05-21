using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zucchinimvc.Migrations
{
    /// <inheritdoc />
    public partial class UserLikedArticles_addArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_UserLikedArticles_Article_ArticleId",
                table: "UserLikedArticles",
                column: "ArticleId",
                principalTable: "Article",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLikedArticles_Article_ArticleId",
                table: "UserLikedArticles");
        }
    }
}
