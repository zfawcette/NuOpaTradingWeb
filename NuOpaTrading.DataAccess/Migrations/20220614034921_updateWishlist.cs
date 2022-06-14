using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NuOpaTrading.DataAccess.Migrations
{
    public partial class updateWishlist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishLists_Games_GameId",
                table: "WishLists");

            migrationBuilder.DropIndex(
                name: "IX_WishLists_GameId",
                table: "WishLists");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "WishLists",
                newName: "GameID");

            migrationBuilder.AlterColumn<long>(
                name: "GameID",
                table: "WishLists",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GameID",
                table: "WishLists",
                newName: "GameId");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "WishLists",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_WishLists_GameId",
                table: "WishLists",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_WishLists_Games_GameId",
                table: "WishLists",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
