using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addlegalteammembernotestableandremovefinalreportsandmergeitscontentwithincidenttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_incidents_app_user_field_researcher_id",
                table: "incidents");

            migrationBuilder.DropTable(
                name: "legal_reviews");

            migrationBuilder.DropTable(
                name: "final_incident_reports");

            migrationBuilder.AddColumn<bool>(
                name: "documentation_consent",
                table: "incidents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "legal_team_member_id",
                table: "incidents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "publication_consent",
                table: "incidents",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "activities",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    made_by_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_activities", x => x.id);
                    table.ForeignKey(
                        name: "fk_activities_app_user_made_by_id",
                        column: x => x.made_by_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "legal_team_member_notes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    creation_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    incident_id = table.Column<int>(type: "integer", nullable: false),
                    legal_team_member_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_legal_team_member_notes", x => x.id);
                    table.ForeignKey(
                        name: "fk_legal_team_member_notes_app_user_legal_team_member_id",
                        column: x => x.legal_team_member_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_legal_team_member_notes_incidents_incident_id",
                        column: x => x.incident_id,
                        principalTable: "incidents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_incidents_legal_team_member_id",
                table: "incidents",
                column: "legal_team_member_id");

            migrationBuilder.CreateIndex(
                name: "ix_activities_made_by_id",
                table: "activities",
                column: "made_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_legal_team_member_notes_incident_id",
                table: "legal_team_member_notes",
                column: "incident_id");

            migrationBuilder.CreateIndex(
                name: "ix_legal_team_member_notes_legal_team_member_id",
                table: "legal_team_member_notes",
                column: "legal_team_member_id");

            migrationBuilder.AddForeignKey(
                name: "fk_incidents_asp_net_users_field_researcher_id",
                table: "incidents",
                column: "field_researcher_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_incidents_asp_net_users_legal_team_member_id",
                table: "incidents",
                column: "legal_team_member_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_incidents_asp_net_users_field_researcher_id",
                table: "incidents");

            migrationBuilder.DropForeignKey(
                name: "fk_incidents_asp_net_users_legal_team_member_id",
                table: "incidents");

            migrationBuilder.DropTable(
                name: "activities");

            migrationBuilder.DropTable(
                name: "legal_team_member_notes");

            migrationBuilder.DropIndex(
                name: "ix_incidents_legal_team_member_id",
                table: "incidents");

            migrationBuilder.DropColumn(
                name: "documentation_consent",
                table: "incidents");

            migrationBuilder.DropColumn(
                name: "legal_team_member_id",
                table: "incidents");

            migrationBuilder.DropColumn(
                name: "publication_consent",
                table: "incidents");

            migrationBuilder.CreateTable(
                name: "final_incident_reports",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    field_researcher_id = table.Column<string>(type: "text", nullable: false),
                    incident_id = table.Column<int>(type: "integer", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    documentation_consent = table.Column<bool>(type: "boolean", nullable: false),
                    publication_consent = table.Column<bool>(type: "boolean", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_final_incident_reports", x => x.id);
                    table.ForeignKey(
                        name: "fk_final_incident_reports_app_user_field_researcher_id",
                        column: x => x.field_researcher_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_final_incident_reports_incidents_incident_id",
                        column: x => x.incident_id,
                        principalTable: "incidents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "legal_reviews",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    final_incident_report_id = table.Column<int>(type: "integer", nullable: false),
                    legal_team_member_id = table.Column<string>(type: "text", nullable: false),
                    review_content = table.Column<string>(type: "text", nullable: false),
                    review_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_legal_reviews", x => x.id);
                    table.ForeignKey(
                        name: "fk_legal_reviews_app_user_legal_team_member_id",
                        column: x => x.legal_team_member_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_legal_reviews_final_incident_reports_final_incident_report_",
                        column: x => x.final_incident_report_id,
                        principalTable: "final_incident_reports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_final_incident_reports_field_researcher_id",
                table: "final_incident_reports",
                column: "field_researcher_id");

            migrationBuilder.CreateIndex(
                name: "ix_final_incident_reports_incident_id",
                table: "final_incident_reports",
                column: "incident_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_legal_reviews_final_incident_report_id",
                table: "legal_reviews",
                column: "final_incident_report_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_legal_reviews_legal_team_member_id",
                table: "legal_reviews",
                column: "legal_team_member_id");

            migrationBuilder.AddForeignKey(
                name: "fk_incidents_app_user_field_researcher_id",
                table: "incidents",
                column: "field_researcher_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
