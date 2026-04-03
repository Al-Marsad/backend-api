using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class updatelocationstableandinitialIncidentReportsttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "coordinates",
                table: "locations");

            migrationBuilder.AddColumn<double>(
                name: "location_lat",
                table: "locations",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "location_lng",
                table: "locations",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "initial_description",
                table: "initial_incident_reports",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AddColumn<string>(
                name: "location_label",
                table: "initial_incident_reports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "location_lat",
                table: "initial_incident_reports",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "location_lng",
                table: "initial_incident_reports",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "witness_name",
                table: "initial_incident_reports",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "witness_phone",
                table: "initial_incident_reports",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "location_lat",
                table: "locations");

            migrationBuilder.DropColumn(
                name: "location_lng",
                table: "locations");

            migrationBuilder.DropColumn(
                name: "location_label",
                table: "initial_incident_reports");

            migrationBuilder.DropColumn(
                name: "location_lat",
                table: "initial_incident_reports");

            migrationBuilder.DropColumn(
                name: "location_lng",
                table: "initial_incident_reports");

            migrationBuilder.DropColumn(
                name: "witness_name",
                table: "initial_incident_reports");

            migrationBuilder.DropColumn(
                name: "witness_phone",
                table: "initial_incident_reports");

            migrationBuilder.AddColumn<string>(
                name: "coordinates",
                table: "locations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "initial_description",
                table: "initial_incident_reports",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
