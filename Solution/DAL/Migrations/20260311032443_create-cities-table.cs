using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class createcitiestable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InitialIncidentReport_AspNetUsers_CitizenReporterId",
                table: "InitialIncidentReport");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InitialIncidentReport",
                table: "InitialIncidentReport");

            migrationBuilder.RenameTable(
                name: "InitialIncidentReport",
                newName: "InitialIncidentReports");

            migrationBuilder.RenameIndex(
                name: "IX_InitialIncidentReport_CitizenReporterId",
                table: "InitialIncidentReports",
                newName: "IX_InitialIncidentReports_CitizenReporterId");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "FieldResearcherInfos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_InitialIncidentReports",
                table: "InitialIncidentReports",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FieldResearcherInfos_CityId",
                table: "FieldResearcherInfos",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_FieldResearcherInfos_Cities_CityId",
                table: "FieldResearcherInfos",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InitialIncidentReports_AspNetUsers_CitizenReporterId",
                table: "InitialIncidentReports",
                column: "CitizenReporterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FieldResearcherInfos_Cities_CityId",
                table: "FieldResearcherInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_InitialIncidentReports_AspNetUsers_CitizenReporterId",
                table: "InitialIncidentReports");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_FieldResearcherInfos_CityId",
                table: "FieldResearcherInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InitialIncidentReports",
                table: "InitialIncidentReports");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "FieldResearcherInfos");

            migrationBuilder.RenameTable(
                name: "InitialIncidentReports",
                newName: "InitialIncidentReport");

            migrationBuilder.RenameIndex(
                name: "IX_InitialIncidentReports_CitizenReporterId",
                table: "InitialIncidentReport",
                newName: "IX_InitialIncidentReport_CitizenReporterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InitialIncidentReport",
                table: "InitialIncidentReport",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InitialIncidentReport_AspNetUsers_CitizenReporterId",
                table: "InitialIncidentReport",
                column: "CitizenReporterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
