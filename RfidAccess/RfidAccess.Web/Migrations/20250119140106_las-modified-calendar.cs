using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RfidAccess.Web.Migrations
{
    /// <inheritdoc />
    public partial class lasmodifiedcalendar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "WeekTimeSlots",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "WeekTimeSlots");
        }
    }
}
