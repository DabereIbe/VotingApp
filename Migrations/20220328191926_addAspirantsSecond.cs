using Microsoft.EntityFrameworkCore.Migrations;

namespace VotingApp.Migrations
{
    public partial class addAspirantsSecond : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "Aspirants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Aspirants_PositionId",
                table: "Aspirants",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Aspirants_Positions_PositionId",
                table: "Aspirants",
                column: "PositionId",
                principalTable: "Positions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aspirants_Positions_PositionId",
                table: "Aspirants");

            migrationBuilder.DropIndex(
                name: "IX_Aspirants_PositionId",
                table: "Aspirants");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "Aspirants");
        }
    }
}
