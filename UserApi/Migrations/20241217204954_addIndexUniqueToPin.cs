using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserApi.Migrations
{
    /// <inheritdoc />
    public partial class addIndexUniqueToPin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_Pin",
                table: "Users",
                column: "Pin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserParents_PinFather",
                table: "UserParents",
                column: "PinFather",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserParents_PinMother",
                table: "UserParents",
                column: "PinMother",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserChildren_Pin",
                table: "UserChildren",
                column: "Pin",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Pin",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserParents_PinFather",
                table: "UserParents");

            migrationBuilder.DropIndex(
                name: "IX_UserParents_PinMother",
                table: "UserParents");

            migrationBuilder.DropIndex(
                name: "IX_UserChildren_Pin",
                table: "UserChildren");
        }
    }
}
