using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateevidencestable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "url",
                table: "evidences",
                newName: "cloudinary_url");

            migrationBuilder.AddColumn<string>(
                name: "cloudinary_public_id",
                table: "evidences",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cloudinary_public_id",
                table: "evidences");

            migrationBuilder.RenameColumn(
                name: "cloudinary_url",
                table: "evidences",
                newName: "url");
        }
    }
}
