using Microsoft.EntityFrameworkCore.Migrations;

namespace DT102G_WeightApp.Data.Migrations
{
    public partial class NavigationReferense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_WeightModelId",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_WeightModelId",
                table: "Comments",
                column: "WeightModelId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_WeightModelId",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_WeightModelId",
                table: "Comments",
                column: "WeightModelId");
        }
    }
}
