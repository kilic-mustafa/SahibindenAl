using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SahibindenAl.Migrations
{
    /// <inheritdoc />
    public partial class AddTurkishCollationToAdvertTexts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryPropertyOptions_CategoryPropertyOptions_CategoryPro~",
                table: "CategoryPropertyOptions");

            migrationBuilder.DropIndex(
                name: "IX_CategoryPropertyOptions_CategoryPropertyOptionId",
                table: "CategoryPropertyOptions");

            migrationBuilder.DropColumn(
                name: "CategoryPropertyOptionId",
                table: "CategoryPropertyOptions");

            migrationBuilder.Sql(@"
                ALTER TABLE Adverts
                ALTER COLUMN Title NVARCHAR(200)
                COLLATE Turkish_CI_AS;

                ALTER TABLE Adverts
                ALTER COLUMN Description NVARCHAR(MAX)
                COLLATE Turkish_CI_AS;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryPropertyOptionId",
                table: "CategoryPropertyOptions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryPropertyOptions_CategoryPropertyOptionId",
                table: "CategoryPropertyOptions",
                column: "CategoryPropertyOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryPropertyOptions_CategoryPropertyOptions_CategoryPro~",
                table: "CategoryPropertyOptions",
                column: "CategoryPropertyOptionId",
                principalTable: "CategoryPropertyOptions",
                principalColumn: "Id");
        }
    }
}
