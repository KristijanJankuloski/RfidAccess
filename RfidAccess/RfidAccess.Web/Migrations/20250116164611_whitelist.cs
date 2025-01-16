using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RfidAccess.Web.Migrations
{
    /// <inheritdoc />
    public partial class whitelist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWhitelisted",
                table: "People",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Records_Code_Time",
                table: "Records",
                columns: new[] { "Code", "Time" });

            migrationBuilder.CreateIndex(
                name: "IX_People_Code",
                table: "People",
                column: "Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Records_Code_Time",
                table: "Records");

            migrationBuilder.DropIndex(
                name: "IX_People_Code",
                table: "People");

            migrationBuilder.DropColumn(
                name: "IsWhitelisted",
                table: "People");
        }
    }
}
