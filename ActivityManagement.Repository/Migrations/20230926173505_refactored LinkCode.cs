using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivityManagement.Repository.Migrations
{
    public partial class refactoredLinkCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "LinkCodes");

            migrationBuilder.AddColumn<string>(
                name: "DateFrom",
                table: "LinkCodes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DateTo",
                table: "LinkCodes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "LinkCodes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateFrom",
                table: "LinkCodes");

            migrationBuilder.DropColumn(
                name: "DateTo",
                table: "LinkCodes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LinkCodes");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "LinkCodes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
