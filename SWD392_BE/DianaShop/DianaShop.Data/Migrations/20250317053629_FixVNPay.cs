using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DianaShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixVNPay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_VnPayTransaction_VnPayTransactionTransactionId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_VnPayTransactionTransactionId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "RoutineStep");

            migrationBuilder.DropColumn(
                name: "VnPayTransactionTransactionId",
                table: "Payment");

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "VnPayTransaction",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VnPayTransaction_PaymentId",
                table: "VnPayTransaction",
                column: "PaymentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VnPayTransaction_Payment_PaymentId",
                table: "VnPayTransaction",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VnPayTransaction_Payment_PaymentId",
                table: "VnPayTransaction");

            migrationBuilder.DropIndex(
                name: "IX_VnPayTransaction_PaymentId",
                table: "VnPayTransaction");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "VnPayTransaction");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "RoutineStep",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "VnPayTransactionTransactionId",
                table: "Payment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_VnPayTransactionTransactionId",
                table: "Payment",
                column: "VnPayTransactionTransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_VnPayTransaction_VnPayTransactionTransactionId",
                table: "Payment",
                column: "VnPayTransactionTransactionId",
                principalTable: "VnPayTransaction",
                principalColumn: "TransactionId");
        }
    }
}
