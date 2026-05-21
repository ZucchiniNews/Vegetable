using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zucchinimvc.Migrations
{
    /// <inheritdoc />
    public partial class RemoveArticleForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLikedArticles_Article_ArticleId",
                table: "UserLikedArticles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLikedArticles",
                table: "UserLikedArticles");

            migrationBuilder.DropIndex(
                name: "IX_UserLikedArticles_UserId",
                table: "UserLikedArticles");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "UserLikedArticles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLikedArticles",
                table: "UserLikedArticles",
                columns: new[] { "UserId", "ArticleId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLikedArticles_UserId1",
                table: "UserLikedArticles",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikedArticles_AspNetUsers_UserId1",
                table: "UserLikedArticles",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLikedArticles_AspNetUsers_UserId1",
                table: "UserLikedArticles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLikedArticles",
                table: "UserLikedArticles");

            migrationBuilder.DropIndex(
                name: "IX_UserLikedArticles_UserId1",
                table: "UserLikedArticles");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserLikedArticles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLikedArticles",
                table: "UserLikedArticles",
                columns: new[] { "ArticleId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLikedArticles_UserId",
                table: "UserLikedArticles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikedArticles_Article_ArticleId",
                table: "UserLikedArticles",
                column: "ArticleId",
                principalTable: "Article",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
