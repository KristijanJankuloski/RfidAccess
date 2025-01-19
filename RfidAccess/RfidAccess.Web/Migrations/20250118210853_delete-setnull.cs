using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RfidAccess.Web.Migrations
{
    /// <inheritdoc />
    public partial class deletesetnull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ErrorLogs_People_PersonId",
                table: "ErrorLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_People_PersonId",
                table: "Records");

            migrationBuilder.AddForeignKey(
                name: "FK_ErrorLogs_People_PersonId",
                table: "ErrorLogs",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_People_PersonId",
                table: "Records",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ErrorLogs_People_PersonId",
                table: "ErrorLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_People_PersonId",
                table: "Records");

            migrationBuilder.AddForeignKey(
                name: "FK_ErrorLogs_People_PersonId",
                table: "ErrorLogs",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_People_PersonId",
                table: "Records",
                column: "PersonId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
