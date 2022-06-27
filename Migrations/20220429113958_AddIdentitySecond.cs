using Microsoft.EntityFrameworkCore.Migrations;

namespace VotingApp.Migrations
{
    public partial class AddIdentitySecond : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Voted",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Voted",
                table: "AspNetUsers");
        }
    }
}
