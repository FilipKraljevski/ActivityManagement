using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivityManagement.Repository.Migrations
{
    public partial class removedCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "LinkCodes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "LinkCodes",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
