using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DianaShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixBrandNameAndPackaging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BrandName",
                table: "Brand",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Brand",
                newName: "BrandName");
        }
    }
}
