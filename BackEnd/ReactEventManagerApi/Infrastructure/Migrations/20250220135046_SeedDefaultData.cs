using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Category", "City", "Date", "Description", "IsCancelled", "Latitude", "Longitutde", "Title", "Venue" },
                values: new object[,]
                {
                    { "09917fd9-8a99-4613-a1a3-2b58caec9f12", "Food", "Los Angeles", new DateTime(2025, 3, 7, 13, 50, 46, 206, DateTimeKind.Utc).AddTicks(7840), "A celebration of world cuisines and local food vendors.", false, 34.040700000000001, -118.26900000000001, "Food Festival", "LA Convention Center" },
                    { "3e8a617f-57c2-4e5b-825c-1302d2c1237b", "Sports", "Boston", new DateTime(2025, 3, 22, 13, 50, 46, 206, DateTimeKind.Utc).AddTicks(7825), "City-wide marathon for charity.", false, 42.355400000000003, -71.065600000000003, "Marathon", "Boston Commons" },
                    { "67994be0-151d-4ea9-9e89-cfa34524bf84", "Technology", "San Francisco", new DateTime(2025, 3, 20, 13, 50, 46, 206, DateTimeKind.Utc).AddTicks(7809), "Annual technology conference with keynote speakers.", false, 37.784700000000001, -122.4011, "Tech Conference", "Moscone Center" },
                    { "9e62464f-b12a-4afa-96bb-afe7f12cc178", "Art", "Paris", new DateTime(2025, 3, 2, 13, 50, 46, 206, DateTimeKind.Utc).AddTicks(7832), "Showcasing modern and contemporary art.", false, 48.860599999999998, 2.3376000000000001, "Art Exhibition", "Louvre Museum" },
                    { "ace0b7e9-1c82-4395-81cb-8398b1419d08", "Music", "New York", new DateTime(2025, 2, 25, 13, 50, 46, 206, DateTimeKind.Utc).AddTicks(7793), "Live music concert featuring popular bands.", false, 40.750500000000002, -73.993399999999994, "Music Concert", "Madison Square Garden" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: "09917fd9-8a99-4613-a1a3-2b58caec9f12");

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: "3e8a617f-57c2-4e5b-825c-1302d2c1237b");

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: "67994be0-151d-4ea9-9e89-cfa34524bf84");

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: "9e62464f-b12a-4afa-96bb-afe7f12cc178");

            migrationBuilder.DeleteData(
                table: "Activities",
                keyColumn: "Id",
                keyValue: "ace0b7e9-1c82-4395-81cb-8398b1419d08");
        }
    }
}
