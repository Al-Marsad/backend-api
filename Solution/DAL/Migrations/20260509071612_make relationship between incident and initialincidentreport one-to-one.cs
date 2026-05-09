using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class makerelationshipbetweenincidentandinitialincidentreportonetoone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_incidents_initial_incident_report_id",
                table: "incidents");

            migrationBuilder.AlterColumn<int>(
                name: "injury_status",
                table: "personal_victim_testimonies",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "injury_description",
                table: "personal_victim_testimonies",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "incident_id",
                table: "initial_incident_reports",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "area_type",
                table: "incidents",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_incidents_initial_incident_report_id",
                table: "incidents",
                column: "initial_incident_report_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_incidents_initial_incident_report_id",
                table: "incidents");

            migrationBuilder.DropColumn(
                name: "incident_id",
                table: "initial_incident_reports");

            migrationBuilder.AlterColumn<int>(
                name: "injury_status",
                table: "personal_victim_testimonies",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "injury_description",
                table: "personal_victim_testimonies",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "area_type",
                table: "incidents",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "ix_incidents_initial_incident_report_id",
                table: "incidents",
                column: "initial_incident_report_id");
        }
    }
}
