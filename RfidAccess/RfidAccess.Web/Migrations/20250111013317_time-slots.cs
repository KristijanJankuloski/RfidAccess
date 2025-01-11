﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RfidAccess.Web.Migrations
{
    /// <inheritdoc />
    public partial class timeslots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeekTimeSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Monday = table.Column<string>(type: "TEXT", nullable: false),
                    Tuesday = table.Column<string>(type: "TEXT", nullable: false),
                    Wednesday = table.Column<string>(type: "TEXT", nullable: false),
                    Thursday = table.Column<string>(type: "TEXT", nullable: false),
                    Friday = table.Column<string>(type: "TEXT", nullable: false),
                    Saturday = table.Column<string>(type: "TEXT", nullable: false),
                    Sunday = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekTimeSlots", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeekTimeSlots");
        }
    }
}
