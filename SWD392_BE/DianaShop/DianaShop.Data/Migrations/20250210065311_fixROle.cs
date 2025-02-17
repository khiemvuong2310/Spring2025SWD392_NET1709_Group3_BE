using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DianaShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixROle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDelete = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");


            migrationBuilder.CreateTable(
                    name: "UserRoles",
                    columns: table => new
                    {
                        UserId = table.Column<int>(type: "int", nullable: false),
                        RoleId = table.Column<int>(type: "int", nullable: false),
                        Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                        CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                        table.ForeignKey(
                            name: "FK_UserRoles_Roles_RoleId",
                            column: x => x.RoleId,
                            principalTable: "Roles",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                        table.ForeignKey(
                            name: "FK_UserRoles_Users_UserId",
                            column: x => x.UserId,
                            principalTable: "Users",
                            principalColumn: "Id",
                            onDelete: ReferentialAction.Cascade);
                    })
                    .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
