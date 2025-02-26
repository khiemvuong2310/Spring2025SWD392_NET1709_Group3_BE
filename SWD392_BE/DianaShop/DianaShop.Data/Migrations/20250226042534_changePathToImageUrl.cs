using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DianaShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class changePathToImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Images",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Images",
                newName: "Path");
        }
    }
}
