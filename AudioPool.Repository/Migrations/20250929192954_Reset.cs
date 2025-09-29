using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioPool.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Reset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfStart",
                table: "Artists",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                table: "Albums",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SongId",
                table: "Albums",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Albums_SongId",
                table: "Albums",
                column: "SongId");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Songs_SongId",
                table: "Albums",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Songs_SongId",
                table: "Albums");

            migrationBuilder.DropIndex(
                name: "IX_Albums_SongId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "SongId",
                table: "Albums");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfStart",
                table: "Artists",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                table: "Albums",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");
        }
    }
}
