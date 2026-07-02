using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StationeryStore.Mvc.Migrations
{
    /// <inheritdoc />
    public partial class SetRowVersionDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS pgcrypto;");

            migrationBuilder.AlterColumn<byte[]>(
            name: "RowVersion",
            table: "Stationeries",
            nullable: false,
            defaultValueSql: "gen_random_bytes(8)");
                migrationBuilder.Sql(@"
                UPDATE ""Stationeries"" 
                SET ""RowVersion"" = gen_random_bytes(8) 
                WHERE ""RowVersion"" = '\x'::bytea OR ""RowVersion"" IS NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
            name: "RowVersion",
            table: "Stationeries",
            nullable: false,
            defaultValue: Array.Empty<byte>());
        }
    }
}
