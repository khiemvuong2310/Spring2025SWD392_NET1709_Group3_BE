using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DianaShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class deleteSkinTypeIdUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_SkinTypes_SkinTypeId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SkinTypeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SkinTypeId",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SkinTypeId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_SkinTypeId",
                table: "Users",
                column: "SkinTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_SkinTypes_SkinTypeId",
                table: "Users",
                column: "SkinTypeId",
                principalTable: "SkinTypes",
                principalColumn: "Id");
        }
    }
}
