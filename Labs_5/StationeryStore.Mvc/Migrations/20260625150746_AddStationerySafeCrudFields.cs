using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StationeryStore.Mvc.Migrations
{
    /// <inheritdoc />
    public partial class AddStationerySafeCrudFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Stationeries_StationeryId",
                table: "OrderDetails");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Stationeries",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Stationeries",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Stationeries",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Stationeries_Sku",
                table: "Stationeries",
                column: "Sku",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Stationeries_StationeryId",
                table: "OrderDetails",
                column: "StationeryId",
                principalTable: "Stationeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Stationeries_StationeryId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_Stationeries_Sku",
                table: "Stationeries");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Stationeries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Stationeries");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Stationeries");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Stationeries_StationeryId",
                table: "OrderDetails",
                column: "StationeryId",
                principalTable: "Stationeries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
