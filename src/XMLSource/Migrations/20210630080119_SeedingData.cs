using Microsoft.EntityFrameworkCore.Migrations;

namespace XMLSource.Migrations
{
    public partial class SeedingData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Results",
                columns: new[] { "Id", "Pressure", "Temperature" },
                values: new object[] { 1, 108.0, 20.5 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Results",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
