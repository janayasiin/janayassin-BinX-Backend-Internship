using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventHub.Migrations
{
    /// <inheritdoc />
    public partial class SeedEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Capacity", "CategoryId", "CreatedAt", "Description", "EndDate", "Location", "StartDate", "Title" },
                values: new object[,]
                {
                    { 1, 200, 2, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A conference about modern technology and software development.", new DateTime(2026, 9, 10, 17, 0, 0, 0, DateTimeKind.Unspecified), "Nablus", new DateTime(2026, 9, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), "Tech Conference 2026" },
                    { 2, 150, 1, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "An evening featuring live music performances.", new DateTime(2026, 9, 15, 22, 0, 0, 0, DateTimeKind.Unspecified), "Ramallah", new DateTime(2026, 9, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), "Live Music Night" },
                    { 3, 500, 3, new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "A local football championship event.", new DateTime(2026, 9, 20, 20, 0, 0, 0, DateTimeKind.Unspecified), "Nablus", new DateTime(2026, 9, 20, 16, 0, 0, 0, DateTimeKind.Unspecified), "Football Championship" },
                    { 4, 100, 2, new DateTime(2026, 8, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "A meetup for developers to discuss backend and web development.", new DateTime(2026, 10, 5, 15, 0, 0, 0, DateTimeKind.Unspecified), "Nablus", new DateTime(2026, 10, 5, 11, 0, 0, 0, DateTimeKind.Unspecified), "Developer Meetup" },
                    { 5, 1000, 1, new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "A large outdoor music festival.", new DateTime(2026, 10, 12, 23, 0, 0, 0, DateTimeKind.Unspecified), "Ramallah", new DateTime(2026, 10, 12, 17, 0, 0, 0, DateTimeKind.Unspecified), "Summer Music Festival" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
