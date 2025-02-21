using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DianaShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateVoucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_VoucherStorages_VoucherStorageId",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Vouchers_VoucherStorageId",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "VoucherID",
                table: "VoucherStorages");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "VoucherStorageId",
                table: "Vouchers");

            migrationBuilder.CreateTable(
                name: "Storages",
                columns: table => new
                {
                    VoucherId = table.Column<int>(type: "int", nullable: false),
                    VoucherStorageId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storages", x => new { x.VoucherId, x.VoucherStorageId });
                    table.ForeignKey(
                        name: "FK_Storages_VoucherStorages_VoucherStorageId",
                        column: x => x.VoucherStorageId,
                        principalTable: "VoucherStorages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Storages_Vouchers_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Vouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Storages_VoucherStorageId",
                table: "Storages",
                column: "VoucherStorageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Storages");

            migrationBuilder.AddColumn<int>(
                name: "VoucherID",
                table: "VoucherStorages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Vouchers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VoucherStorageId",
                table: "Vouchers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_VoucherStorageId",
                table: "Vouchers",
                column: "VoucherStorageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_VoucherStorages_VoucherStorageId",
                table: "Vouchers",
                column: "VoucherStorageId",
                principalTable: "VoucherStorages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
