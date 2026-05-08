using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addAIclassifyingmodificationondatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_evidences_residences_residence_id",
                table: "evidences");

            migrationBuilder.DropForeignKey(
                name: "fk_incidents_locations_location_id",
                table: "incidents");

            migrationBuilder.DropTable(
                name: "locations");

            migrationBuilder.DropTable(
                name: "residences");

            migrationBuilder.DropIndex(
                name: "ix_evidences_residence_id",
                table: "evidences");

            migrationBuilder.DropColumn(
                name: "hardest_moment",
                table: "personal_victim_testimonies");

            migrationBuilder.DropColumn(
                name: "residence_id",
                table: "evidences");

            migrationBuilder.RenameColumn(
                name: "international_demand",
                table: "personal_victim_testimonies",
                newName: "injury_description");

            migrationBuilder.RenameColumn(
                name: "location_id",
                table: "incidents",
                newName: "city_id");

            migrationBuilder.RenameIndex(
                name: "ix_incidents_location_id",
                table: "incidents",
                newName: "ix_incidents_city_id");

            migrationBuilder.AlterColumn<int>(
                name: "marital_status",
                table: "victims",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "fmaily_size",
                table: "victims",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "personal_narrative",
                table: "personal_victim_testimonies",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "injury_status",
                table: "personal_victim_testimonies",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "witness_details",
                table: "incidents",
                type: "text",
                nullable: true,
                oldClrType: typeof(Dictionary<string, object>),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<int>(
                name: "witness_count",
                table: "incidents",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "detailed_description",
                table: "incidents",
                type: "text",
                nullable: false,
                oldClrType: typeof(Dictionary<string, object>),
                oldType: "jsonb");

            migrationBuilder.AddColumn<int>(
                name: "area_class",
                table: "incidents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "area_name",
                table: "incidents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "area_type",
                table: "incidents",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "creation_date",
                table: "incidents",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "location_description",
                table: "incidents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "location_lat",
                table: "incidents",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "location_lng",
                table: "incidents",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "perpetrator_description",
                table: "incidents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "questionnaire_json",
                table: "incidents",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "sensitivity_score",
                table: "incidents",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "incident_class_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_incident_class_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    question_body = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_questions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "incident_classes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    incident_class_type_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_incident_classes", x => x.id);
                    table.ForeignKey(
                        name: "fk_incident_classes_incident_class_types_incident_class_type_id",
                        column: x => x.incident_class_type_id,
                        principalTable: "incident_class_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "incident_class_type_question",
                columns: table => new
                {
                    incident_class_types_id = table.Column<int>(type: "integer", nullable: false),
                    questions_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_incident_class_type_question", x => new { x.incident_class_types_id, x.questions_id });
                    table.ForeignKey(
                        name: "fk_incident_class_type_question_incident_class_types_incident_",
                        column: x => x.incident_class_types_id,
                        principalTable: "incident_class_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_incident_class_type_question_questions_questions_id",
                        column: x => x.questions_id,
                        principalTable: "questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_incident_class_type_question_questions_id",
                table: "incident_class_type_question",
                column: "questions_id");

            migrationBuilder.CreateIndex(
                name: "ix_incident_classes_incident_class_type_id",
                table: "incident_classes",
                column: "incident_class_type_id");

            migrationBuilder.AddForeignKey(
                name: "fk_incidents_cities_city_id",
                table: "incidents",
                column: "city_id",
                principalTable: "cities",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_incidents_cities_city_id",
                table: "incidents");

            migrationBuilder.DropTable(
                name: "incident_class_type_question");

            migrationBuilder.DropTable(
                name: "incident_classes");

            migrationBuilder.DropTable(
                name: "questions");

            migrationBuilder.DropTable(
                name: "incident_class_types");

            migrationBuilder.DropColumn(
                name: "injury_status",
                table: "personal_victim_testimonies");

            migrationBuilder.DropColumn(
                name: "area_class",
                table: "incidents");

            migrationBuilder.DropColumn(
                name: "area_name",
                table: "incidents");

            migrationBuilder.DropColumn(
                name: "area_type",
                table: "incidents");

            migrationBuilder.DropColumn(
                name: "creation_date",
                table: "incidents");

            migrationBuilder.DropColumn(
                name: "location_description",
                table: "incidents");

            migrationBuilder.DropColumn(
                name: "location_lat",
                table: "incidents");

            migrationBuilder.DropColumn(
                name: "location_lng",
                table: "incidents");

            migrationBuilder.DropColumn(
                name: "perpetrator_description",
                table: "incidents");

            migrationBuilder.DropColumn(
                name: "questionnaire_json",
                table: "incidents");

            migrationBuilder.DropColumn(
                name: "sensitivity_score",
                table: "incidents");

            migrationBuilder.RenameColumn(
                name: "injury_description",
                table: "personal_victim_testimonies",
                newName: "international_demand");

            migrationBuilder.RenameColumn(
                name: "city_id",
                table: "incidents",
                newName: "location_id");

            migrationBuilder.RenameIndex(
                name: "ix_incidents_city_id",
                table: "incidents",
                newName: "ix_incidents_location_id");

            migrationBuilder.AlterColumn<int>(
                name: "marital_status",
                table: "victims",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "fmaily_size",
                table: "victims",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "personal_narrative",
                table: "personal_victim_testimonies",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "hardest_moment",
                table: "personal_victim_testimonies",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<Dictionary<string, object>>(
                name: "witness_details",
                table: "incidents",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "witness_count",
                table: "incidents",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<Dictionary<string, object>>(
                name: "detailed_description",
                table: "incidents",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "residence_id",
                table: "evidences",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    city_id = table.Column<int>(type: "integer", nullable: false),
                    area_class = table.Column<int>(type: "integer", nullable: true),
                    area_name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    location_lat = table.Column<double>(type: "double precision", nullable: false),
                    location_lng = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_locations", x => x.id);
                    table.ForeignKey(
                        name: "fk_locations_cities_city_id",
                        column: x => x.city_id,
                        principalTable: "cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "residences",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    incident_id = table.Column<int>(type: "integer", nullable: false),
                    housing_type = table.Column<int>(type: "integer", nullable: false),
                    ownership_status = table.Column<int>(type: "integer", nullable: false),
                    residence_start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    was_permanently_occupied = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_residences", x => x.id);
                    table.ForeignKey(
                        name: "fk_residences_incidents_incident_id",
                        column: x => x.incident_id,
                        principalTable: "incidents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_evidences_residence_id",
                table: "evidences",
                column: "residence_id");

            migrationBuilder.CreateIndex(
                name: "ix_locations_city_id",
                table: "locations",
                column: "city_id");

            migrationBuilder.CreateIndex(
                name: "ix_residences_incident_id",
                table: "residences",
                column: "incident_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_evidences_residences_residence_id",
                table: "evidences",
                column: "residence_id",
                principalTable: "residences",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_incidents_locations_location_id",
                table: "incidents",
                column: "location_id",
                principalTable: "locations",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
