using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManager.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCLIENTNewInfos2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Clients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Division",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OccupationGroup",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkReferenceNumber",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Division",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "OccupationGroup",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "WorkReferenceNumber",
                table: "Clients");
        }
    }
}
