using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarshipShop.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddManufacturerAndPriceToEngineAndFtlDrive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "manufacturer",
                table: "ftl_drives",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "ftl_drives",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "manufacturer",
                table: "engines",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "engines",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "manufacturer",
                table: "ftl_drives");

            migrationBuilder.DropColumn(
                name: "price",
                table: "ftl_drives");

            migrationBuilder.DropColumn(
                name: "manufacturer",
                table: "engines");

            migrationBuilder.DropColumn(
                name: "price",
                table: "engines");
        }
    }
}
