using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManager.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCLIENTNewInfos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmploymentType",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "HireDate",
                table: "Clients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Manager",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Salary",
                table: "Clients",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "EmploymentType",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "HireDate",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Manager",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Clients");
        }
    }
}
