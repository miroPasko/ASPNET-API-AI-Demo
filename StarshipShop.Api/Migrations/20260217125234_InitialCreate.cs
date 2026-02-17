using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StarshipShop.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "engines",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    maximum_speed = table.Column<double>(type: "double precision", nullable: false),
                    fuel_usage = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_engines", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ftl_drives",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    maximum_speed = table.Column<double>(type: "double precision", nullable: false),
                    fuel_usage = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ftl_drives", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "starships",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    manufacturer = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    engine_id = table.Column<int>(type: "integer", nullable: false),
                    ftl_capable = table.Column<bool>(type: "boolean", nullable: false),
                    ftl_drive_id = table.Column<int>(type: "integer", nullable: true),
                    total_crew = table.Column<int>(type: "integer", nullable: false),
                    total_capacity = table.Column<int>(type: "integer", nullable: false),
                    icon_picture_file_path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    starship_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_starships", x => x.id);
                    table.ForeignKey(
                        name: "FK_starships_engines_engine_id",
                        column: x => x.engine_id,
                        principalTable: "engines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_starships_ftl_drives_ftl_drive_id",
                        column: x => x.ftl_drive_id,
                        principalTable: "ftl_drives",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "payment_details",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    card_number = table.Column<string>(type: "character varying(19)", maxLength: 19, nullable: false),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_details", x => x.id);
                    table.ForeignKey(
                        name: "FK_payment_details_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cargo_vessels",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    cargo_type = table.Column<string>(type: "text", nullable: false),
                    total_cargo_capacity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cargo_vessels", x => x.id);
                    table.ForeignKey(
                        name: "FK_cargo_vessels_starships_id",
                        column: x => x.id,
                        principalTable: "starships",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "private_vessels",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    vessel_type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_private_vessels", x => x.id);
                    table.ForeignKey(
                        name: "FK_private_vessels_starships_id",
                        column: x => x.id,
                        principalTable: "starships",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "public_transport_vessels",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    transport_class = table.Column<string>(type: "text", nullable: false),
                    total_passengers = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_public_transport_vessels", x => x.id);
                    table.ForeignKey(
                        name: "FK_public_transport_vessels_starships_id",
                        column: x => x.id,
                        principalTable: "starships",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sales",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    starship_id = table.Column<int>(type: "integer", nullable: false),
                    payment_details_id = table.Column<int>(type: "integer", nullable: false),
                    purchased_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sales", x => x.id);
                    table.ForeignKey(
                        name: "FK_sales_payment_details_payment_details_id",
                        column: x => x.payment_details_id,
                        principalTable: "payment_details",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sales_starships_starship_id",
                        column: x => x.starship_id,
                        principalTable: "starships",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_sales_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_payment_details_user_id",
                table: "payment_details",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_sales_payment_details_id",
                table: "sales",
                column: "payment_details_id");

            migrationBuilder.CreateIndex(
                name: "IX_sales_starship_id",
                table: "sales",
                column: "starship_id");

            migrationBuilder.CreateIndex(
                name: "IX_sales_user_id",
                table: "sales",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_starships_engine_id",
                table: "starships",
                column: "engine_id");

            migrationBuilder.CreateIndex(
                name: "IX_starships_ftl_drive_id",
                table: "starships",
                column: "ftl_drive_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cargo_vessels");

            migrationBuilder.DropTable(
                name: "private_vessels");

            migrationBuilder.DropTable(
                name: "public_transport_vessels");

            migrationBuilder.DropTable(
                name: "sales");

            migrationBuilder.DropTable(
                name: "payment_details");

            migrationBuilder.DropTable(
                name: "starships");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "engines");

            migrationBuilder.DropTable(
                name: "ftl_drives");
        }
    }
}
