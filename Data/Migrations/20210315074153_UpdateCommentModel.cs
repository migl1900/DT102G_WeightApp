using Microsoft.EntityFrameworkCore.Migrations;

namespace DT102G_WeightApp.Data.Migrations
{
    public partial class UpdateCommentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Weights_WeightId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "WeightId",
                table: "Comments",
                newName: "WeightModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_WeightId",
                table: "Comments",
                newName: "IX_Comments_WeightModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Weights_WeightModelId",
                table: "Comments",
                column: "WeightModelId",
                principalTable: "Weights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Weights_WeightModelId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "WeightModelId",
                table: "Comments",
                newName: "WeightId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_WeightModelId",
                table: "Comments",
                newName: "IX_Comments_WeightId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Weights_WeightId",
                table: "Comments",
                column: "WeightId",
                principalTable: "Weights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
