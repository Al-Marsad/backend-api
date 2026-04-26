using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addAppRoleandAppUserRoleclasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_initial_incident_reports_app_user_citizen_reporter_id",
                table: "initial_incident_reports");

            migrationBuilder.DropForeignKey(
                name: "fk_initial_incident_reports_app_user_field_researcher_id",
                table: "initial_incident_reports");

            migrationBuilder.AddForeignKey(
                name: "fk_initial_incident_reports_asp_net_users_citizen_reporter_id",
                table: "initial_incident_reports",
                column: "citizen_reporter_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_initial_incident_reports_asp_net_users_field_researcher_id",
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
                name: "fk_initial_incident_reports_asp_net_users_citizen_reporter_id",
                table: "initial_incident_reports");

            migrationBuilder.DropForeignKey(
                name: "fk_initial_incident_reports_asp_net_users_field_researcher_id",
                table: "initial_incident_reports");

            migrationBuilder.AddForeignKey(
                name: "fk_initial_incident_reports_app_user_citizen_reporter_id",
                table: "initial_incident_reports",
                column: "citizen_reporter_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_initial_incident_reports_app_user_field_researcher_id",
                table: "initial_incident_reports",
                column: "field_researcher_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
