using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DianaShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateVoucherTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Vouchers_VoucherStorages_VoucherStorageID",
            //    table: "Vouchers");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_VoucherStorages_Users_UserID",
            //    table: "VoucherStorages");

            //migrationBuilder.RenameColumn(
            //    name: "VoucherStorageID",
            //    table: "Vouchers",
            //    newName: "VoucherStorageId");

            migrationBuilder.RenameIndex(
                name: "IX_Vouchers_VoucherStorageID",
                table: "Vouchers",
                newName: "IX_Vouchers_VoucherStorageId");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "VoucherStorages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "VoucherID",
            //    table: "VoucherStorages",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.InsertData(
            //    table: "Roles",
            //    columns: new[] { "Id", "IsDelete", "Name" },
            //    values: new object[,]
            //    {
            //        { 1, false, "Manager" },
            //        { 2, false, "Staff" },
            //        { 3, false, "Customer" }
            //    });

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_VoucherStorages_VoucherStorageId",
                table: "Vouchers",
                column: "VoucherStorageId",
                principalTable: "VoucherStorages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherStorages_Users_UserID",
                table: "VoucherStorages",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_VoucherStorages_VoucherStorageId",
                table: "Vouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherStorages_Users_UserID",
                table: "VoucherStorages");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "VoucherID",
                table: "VoucherStorages");

            migrationBuilder.RenameColumn(
                name: "VoucherStorageId",
                table: "Vouchers",
                newName: "VoucherStorageID");

            migrationBuilder.RenameIndex(
                name: "IX_Vouchers_VoucherStorageId",
                table: "Vouchers",
                newName: "IX_Vouchers_VoucherStorageID");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "VoucherStorages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_VoucherStorages_VoucherStorageID",
                table: "Vouchers",
                column: "VoucherStorageID",
                principalTable: "VoucherStorages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherStorages_Users_UserID",
                table: "VoucherStorages",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
