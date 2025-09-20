using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AirQuality.OpenAQ.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Locality = table.Column<string>(type: "text", nullable: true),
                    Timezone = table.Column<string>(type: "text", nullable: true),
                    Country_Id = table.Column<int>(type: "integer", nullable: true),
                    Country_Code = table.Column<string>(type: "text", nullable: true),
                    Country_Name = table.Column<string>(type: "text", nullable: true),
                    Country_DatetimeFirst = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Country_DatetimeLast = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Owner_Id = table.Column<int>(type: "integer", nullable: true),
                    Owner_Name = table.Column<string>(type: "text", nullable: true),
                    Provider_Id = table.Column<int>(type: "integer", nullable: true),
                    Provider_Name = table.Column<string>(type: "text", nullable: true),
                    IsMobile = table.Column<bool>(type: "boolean", nullable: false),
                    IsMonitor = table.Column<bool>(type: "boolean", nullable: false),
                    Coordinates_Id = table.Column<int>(type: "integer", nullable: true),
                    Coordinates_Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Coordinates_Longitude = table.Column<double>(type: "double precision", nullable: true),
                    Bounds = table.Column<List<double>>(type: "double precision[]", nullable: true),
                    Distance = table.Column<double>(type: "double precision", nullable: false),
                    DatetimeFirst_Id = table.Column<int>(type: "integer", nullable: true),
                    DatetimeFirst_Utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DatetimeFirst_Local = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DatetimeLast_Id = table.Column<int>(type: "integer", nullable: true),
                    DatetimeLast_Utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DatetimeLast_Local = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parameters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Units = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations_Instruments",
                columns: table => new
                {
                    InstrumentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    InstrumentName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations_Instruments", x => new { x.LocationId, x.InstrumentId });
                    table.ForeignKey(
                        name: "FK_Locations_Instruments_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations_Licenses",
                columns: table => new
                {
                    LicenseId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LocationId = table.Column<int>(type: "integer", nullable: false),
                    LicenseName = table.Column<string>(type: "text", nullable: true),
                    AttributionId = table.Column<int>(type: "integer", nullable: true),
                    AttributionName = table.Column<string>(type: "text", nullable: true),
                    AttributionUrl = table.Column<string>(type: "text", nullable: true),
                    DateFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations_Licenses", x => new { x.LocationId, x.LicenseId });
                    table.ForeignKey(
                        name: "FK_Locations_Licenses_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocationSensorDTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParameterId = table.Column<int>(type: "integer", nullable: true),
                    LocationId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationSensorDTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationSensorDTO_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationSensorDTO_Parameters_ParameterId",
                        column: x => x.ParameterId,
                        principalTable: "Parameters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationSensorDTO_LocationId",
                table: "LocationSensorDTO",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationSensorDTO_ParameterId",
                table: "LocationSensorDTO",
                column: "ParameterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locations_Instruments");

            migrationBuilder.DropTable(
                name: "Locations_Licenses");

            migrationBuilder.DropTable(
                name: "LocationSensorDTO");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Parameters");
        }
    }
}
