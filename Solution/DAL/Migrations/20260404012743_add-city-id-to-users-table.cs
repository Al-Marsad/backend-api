using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addcityidtouserstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_users_cities_city_id",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "city_id",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_users_cities_city_id",
                table: "AspNetUsers",
                column: "city_id",
                principalTable: "cities",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_users_cities_city_id",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "city_id",
                table: "AspNetUsers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_users_cities_city_id",
                table: "AspNetUsers",
                column: "city_id",
                principalTable: "cities",
                principalColumn: "id");
        }
    }
}
