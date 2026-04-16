using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addcityIdforeginkeytointialincidentreport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "location_label",
                table: "initial_incident_reports");

            migrationBuilder.AddColumn<int>(
                name: "city_id",
                table: "initial_incident_reports",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "ix_initial_incident_reports_city_id",
                table: "initial_incident_reports",
                column: "city_id");

            migrationBuilder.AddForeignKey(
                name: "fk_initial_incident_reports_cities_city_id",
                table: "initial_incident_reports",
                column: "city_id",
                principalTable: "cities",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_initial_incident_reports_cities_city_id",
                table: "initial_incident_reports");

            migrationBuilder.DropIndex(
                name: "ix_initial_incident_reports_city_id",
                table: "initial_incident_reports");

            migrationBuilder.DropColumn(
                name: "city_id",
                table: "initial_incident_reports");

            migrationBuilder.AddColumn<string>(
                name: "location_label",
                table: "initial_incident_reports",
                type: "text",
                nullable: true);
        }
    }
}
