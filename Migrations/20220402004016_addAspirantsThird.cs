using Microsoft.EntityFrameworkCore.Migrations;

namespace VotingApp.Migrations
{
    public partial class addAspirantsThird : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Aspirants");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Aspirants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
