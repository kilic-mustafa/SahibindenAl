using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SahibindenAl.Migrations
{
    /// <inheritdoc />
    public partial class AddParentPropertyToCategoryPropertyKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentPropertyId",
                table: "CategoryPropertyKeys",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryPropertyKeys_ParentPropertyId",
                table: "CategoryPropertyKeys",
                column: "ParentPropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryPropertyKeys_CategoryPropertyKeys_ParentPropertyId",
                table: "CategoryPropertyKeys",
                column: "ParentPropertyId",
                principalTable: "CategoryPropertyKeys",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryPropertyKeys_CategoryPropertyKeys_ParentPropertyId",
                table: "CategoryPropertyKeys");

            migrationBuilder.DropIndex(
                name: "IX_CategoryPropertyKeys_ParentPropertyId",
                table: "CategoryPropertyKeys");

            migrationBuilder.DropColumn(
                name: "ParentPropertyId",
                table: "CategoryPropertyKeys");
        }
    }
}
