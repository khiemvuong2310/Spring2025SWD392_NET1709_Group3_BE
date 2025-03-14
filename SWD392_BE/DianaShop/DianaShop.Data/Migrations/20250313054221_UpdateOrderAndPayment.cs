using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DianaShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderAndPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Vouchers_VoucherID",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Payment");

            migrationBuilder.RenameColumn(
                name: "VoucherID",
                table: "Payment",
                newName: "VnPayTransactionTransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_VoucherID",
                table: "Payment",
                newName: "IX_Payment_VnPayTransactionTransactionId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Orders",
                newName: "Address");

            migrationBuilder.AddColumn<decimal>(
                name: "FinalAmount",
                table: "Orders",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ShipAmount",
                table: "Orders",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VoucherID",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "PaymentMethod",
                columns: new[] { "Id", "IsDelete", "MethodName" },
                values: new object[,]
                {
                    { 1, false, "Cash on Delivery" },
                    { 2, false, "VNPay" }
                });

            migrationBuilder.UpdateData(
                table: "StageStatus",
                keyColumn: "Id",
                keyValue: 3,
                column: "StatusName",
                value: "Waiting");

            migrationBuilder.UpdateData(
                table: "StageStatus",
                keyColumn: "Id",
                keyValue: 4,
                column: "StatusName",
                value: "Shipping");

            migrationBuilder.InsertData(
                table: "StageStatus",
                columns: new[] { "Id", "IsDelete", "StatusName" },
                values: new object[] { 5, false, "Completed" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_VoucherID",
                table: "Orders",
                column: "VoucherID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserID",
                table: "Orders",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Vouchers_VoucherID",
                table: "Orders",
                column: "VoucherID",
                principalTable: "Vouchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_VnPayTransaction_VnPayTransactionTransactionId",
                table: "Payment",
                column: "VnPayTransactionTransactionId",
                principalTable: "VnPayTransaction",
                principalColumn: "TransactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Vouchers_VoucherID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_VnPayTransaction_VnPayTransactionTransactionId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Orders_VoucherID",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "PaymentMethod",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PaymentMethod",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "StageStatus",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "FinalAmount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipAmount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VoucherID",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "VnPayTransactionTransactionId",
                table: "Payment",
                newName: "VoucherID");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_VnPayTransactionTransactionId",
                table: "Payment",
                newName: "IX_Payment_VoucherID");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Orders",
                newName: "Status");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Payment",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "StageStatus",
                keyColumn: "Id",
                keyValue: 3,
                column: "StatusName",
                value: "Shipping");

            migrationBuilder.UpdateData(
                table: "StageStatus",
                keyColumn: "Id",
                keyValue: 4,
                column: "StatusName",
                value: "Completed");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserID",
                table: "Orders",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Vouchers_VoucherID",
                table: "Payment",
                column: "VoucherID",
                principalTable: "Vouchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
