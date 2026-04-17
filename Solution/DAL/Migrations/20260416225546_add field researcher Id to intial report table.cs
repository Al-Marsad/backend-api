using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addfieldresearcherIdtointialreporttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "field_researcher_id",
                table: "initial_incident_reports",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_initial_incident_reports_field_researcher_id",
                table: "initial_incident_reports",
                column: "field_researcher_id");

            migrationBuilder.AddForeignKey(
                name: "fk_initial_incident_reports_app_user_field_researcher_id",
                table: "initial_incident_reports",
                column: "field_researcher_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_initial_incident_reports_app_user_field_researcher_id",
                table: "initial_incident_reports");

            migrationBuilder.DropIndex(
                name: "ix_initial_incident_reports_field_researcher_id",
                table: "initial_incident_reports");

            migrationBuilder.DropColumn(
                name: "field_researcher_id",
                table: "initial_incident_reports");
        }
    }
}
