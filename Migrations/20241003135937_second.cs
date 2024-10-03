using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace microservice_search_ads.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AdModels");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AdModels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
