using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Models.Migrations
{
    /// <inheritdoc />
    public partial class oAuthServerOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AllowedScopes",
                schema: "OAuth",
                table: "OAuthApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GrantTypes",
                schema: "OAuth",
                table: "OAuthApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UsePkce",
                schema: "OAuth",
                table: "OAuthApplications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowedScopes",
                schema: "OAuth",
                table: "OAuthApplications");

            migrationBuilder.DropColumn(
                name: "GrantTypes",
                schema: "OAuth",
                table: "OAuthApplications");

            migrationBuilder.DropColumn(
                name: "UsePkce",
                schema: "OAuth",
                table: "OAuthApplications");
        }
    }
}
