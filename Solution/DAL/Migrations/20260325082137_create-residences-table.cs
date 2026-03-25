using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class createresidencestable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IncidentId",
                table: "Evidences",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResidenceId",
                table: "Evidences",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Residences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HousingType = table.Column<int>(type: "int", nullable: false),
                    OwnershipStatus = table.Column<int>(type: "int", nullable: false),
                    ResidenceStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WasPermanentlyOccupied = table.Column<bool>(type: "bit", nullable: false),
                    IncidentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Residences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Residences_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_IncidentId",
                table: "Evidences",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Evidences_ResidenceId",
                table: "Evidences",
                column: "ResidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Residences_IncidentId",
                table: "Residences",
                column: "IncidentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Evidences_Incidents_IncidentId",
                table: "Evidences",
                column: "IncidentId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Evidences_Residences_ResidenceId",
                table: "Evidences",
                column: "ResidenceId",
                principalTable: "Residences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evidences_Incidents_IncidentId",
                table: "Evidences");

            migrationBuilder.DropForeignKey(
                name: "FK_Evidences_Residences_ResidenceId",
                table: "Evidences");

            migrationBuilder.DropTable(
                name: "Residences");

            migrationBuilder.DropIndex(
                name: "IX_Evidences_IncidentId",
                table: "Evidences");

            migrationBuilder.DropIndex(
                name: "IX_Evidences_ResidenceId",
                table: "Evidences");

            migrationBuilder.DropColumn(
                name: "IncidentId",
                table: "Evidences");

            migrationBuilder.DropColumn(
                name: "ResidenceId",
                table: "Evidences");
        }
    }
}
