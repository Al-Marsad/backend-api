using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class somemodificationsontables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_final_incident_reports_field_researcher_infos_field_researc",
                table: "final_incident_reports");

            migrationBuilder.DropForeignKey(
                name: "fk_incidents_field_researcher_infos_field_researcher_id",
                table: "incidents");

            migrationBuilder.DropForeignKey(
                name: "fk_legal_reviews_app_user_app_user_id",
                table: "legal_reviews");

            migrationBuilder.DropTable(
                name: "field_researcher_infos");

            migrationBuilder.RenameColumn(
                name: "app_user_id",
                table: "legal_reviews",
                newName: "legal_team_member_id");

            migrationBuilder.RenameIndex(
                name: "ix_legal_reviews_app_user_id",
                table: "legal_reviews",
                newName: "ix_legal_reviews_legal_team_member_id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "cities",
                newName: "english_name");

            migrationBuilder.RenameIndex(
                name: "ix_cities_name",
                table: "cities",
                newName: "ix_cities_english_name");

            migrationBuilder.AddColumn<string>(
                name: "arabic_name",
                table: "cities",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "availability_status",
                table: "AspNetUsers",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "city_id",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_cities_arabic_name",
                table: "cities",
                column: "arabic_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_city_id",
                table: "AspNetUsers",
                column: "city_id");

            migrationBuilder.AddForeignKey(
                name: "fk_final_incident_reports_app_user_field_researcher_id",
                table: "final_incident_reports",
                column: "field_researcher_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_incidents_app_user_field_researcher_id",
                table: "incidents",
                column: "field_researcher_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_legal_reviews_app_user_legal_team_member_id",
                table: "legal_reviews",
                column: "legal_team_member_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_users_cities_city_id",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "fk_final_incident_reports_app_user_field_researcher_id",
                table: "final_incident_reports");

            migrationBuilder.DropForeignKey(
                name: "fk_incidents_app_user_field_researcher_id",
                table: "incidents");

            migrationBuilder.DropForeignKey(
                name: "fk_legal_reviews_app_user_legal_team_member_id",
                table: "legal_reviews");

            migrationBuilder.DropIndex(
                name: "ix_cities_arabic_name",
                table: "cities");

            migrationBuilder.DropIndex(
                name: "ix_asp_net_users_city_id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "arabic_name",
                table: "cities");

            migrationBuilder.DropColumn(
                name: "availability_status",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "city_id",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "legal_team_member_id",
                table: "legal_reviews",
                newName: "app_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_legal_reviews_legal_team_member_id",
                table: "legal_reviews",
                newName: "ix_legal_reviews_app_user_id");

            migrationBuilder.RenameColumn(
                name: "english_name",
                table: "cities",
                newName: "name");

            migrationBuilder.RenameIndex(
                name: "ix_cities_english_name",
                table: "cities",
                newName: "ix_cities_name");

            migrationBuilder.CreateTable(
                name: "field_researcher_infos",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    city_id = table.Column<int>(type: "integer", nullable: false),
                    availability_status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_field_researcher_infos", x => x.user_id);
                    table.ForeignKey(
                        name: "fk_field_researcher_infos_app_user_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_field_researcher_infos_cities_city_id",
                        column: x => x.city_id,
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_field_researcher_infos_city_id",
                table: "field_researcher_infos",
                column: "city_id");

            migrationBuilder.AddForeignKey(
                name: "fk_final_incident_reports_field_researcher_infos_field_researc",
                table: "final_incident_reports",
                column: "field_researcher_id",
                principalTable: "field_researcher_infos",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_incidents_field_researcher_infos_field_researcher_id",
                table: "incidents",
                column: "field_researcher_id",
                principalTable: "field_researcher_infos",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_legal_reviews_app_user_app_user_id",
                table: "legal_reviews",
                column: "app_user_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
