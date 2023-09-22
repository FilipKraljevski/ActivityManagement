using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivityManagement.Repository.Migrations
{
    public partial class addedlinkCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LinkCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    Expire = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkCodes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkCodes");
        }
    }
}
