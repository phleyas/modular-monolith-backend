using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AirQuality.OpenAQ.Migrations
{
    /// <inheritdoc />
    public partial class sensorchanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SensorDTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParameterId = table.Column<int>(type: "integer", nullable: true),
                    DatetimeFirst_Id = table.Column<int>(type: "integer", nullable: true),
                    DatetimeFirst_Utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DatetimeFirst_Local = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DatetimeLast_Id = table.Column<int>(type: "integer", nullable: true),
                    DatetimeLast_Utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DatetimeLast_Local = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Coverage_ExpectedCount = table.Column<int>(type: "integer", nullable: true),
                    Coverage_ExpectedInterval = table.Column<string>(type: "text", nullable: true),
                    Coverage_ObservedCount = table.Column<int>(type: "integer", nullable: true),
                    Coverage_ObservedInterval = table.Column<string>(type: "text", nullable: true),
                    Coverage_PercentComplete = table.Column<double>(type: "double precision", nullable: true),
                    Coverage_PercentCoverage = table.Column<double>(type: "double precision", nullable: true),
                    Coverage_DatetimeFrom_Id = table.Column<int>(type: "integer", nullable: true),
                    Coverage_DatetimeFrom_Utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Coverage_DatetimeFrom_Local = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Coverage_DatetimeTo_Id = table.Column<int>(type: "integer", nullable: true),
                    Coverage_DatetimeTo_Utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Coverage_DatetimeTo_Local = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Latest_HasValueMarker = table.Column<bool>(type: "boolean", nullable: true),
                    Latest_Datetime_Id = table.Column<int>(type: "integer", nullable: true),
                    Latest_Datetime_Utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Latest_Datetime_Local = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Latest_Value = table.Column<double>(type: "double precision", nullable: true),
                    Latest_Coordinates_Id = table.Column<int>(type: "integer", nullable: true),
                    Latest_Coordinates_Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Latest_Coordinates_Longitude = table.Column<double>(type: "double precision", nullable: true),
                    Summary_Min = table.Column<double>(type: "double precision", nullable: true),
                    Summary_Q02 = table.Column<double>(type: "double precision", nullable: true),
                    Summary_Q25 = table.Column<double>(type: "double precision", nullable: true),
                    Summary_Median = table.Column<double>(type: "double precision", nullable: true),
                    Summary_Q75 = table.Column<double>(type: "double precision", nullable: true),
                    Summary_Q98 = table.Column<double>(type: "double precision", nullable: true),
                    Summary_Max = table.Column<double>(type: "double precision", nullable: true),
                    Summary_Avg = table.Column<double>(type: "double precision", nullable: true),
                    Summary_Sd = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorDTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorDTO_Parameters_ParameterId",
                        column: x => x.ParameterId,
                        principalTable: "Parameters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SensorDTO_ParameterId",
                table: "SensorDTO",
                column: "ParameterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensorDTO");
        }
    }
}
