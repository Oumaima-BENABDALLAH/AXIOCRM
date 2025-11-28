using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManager.API.Migrations
{
    /// <inheritdoc />
    public partial class Order_MethodPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryMethod_DeliveryMethodId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryMethod",
                table: "DeliveryMethod");

            migrationBuilder.RenameTable(
                name: "DeliveryMethod",
                newName: "DeliveryMethods");

            migrationBuilder.AddColumn<string>(
                name: "CVV",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardHolder",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CashAmount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExpiryDate",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryMethods",
                table: "DeliveryMethods",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryMethods_DeliveryMethodId",
                table: "Orders",
                column: "DeliveryMethodId",
                principalTable: "DeliveryMethods",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryMethods_DeliveryMethodId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryMethods",
                table: "DeliveryMethods");

            migrationBuilder.DropColumn(
                name: "CVV",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CardHolder",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CashAmount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "DeliveryMethods",
                newName: "DeliveryMethod");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryMethod",
                table: "DeliveryMethod",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryMethod_DeliveryMethodId",
                table: "Orders",
                column: "DeliveryMethodId",
                principalTable: "DeliveryMethod",
                principalColumn: "Id");
        }
    }
}
