using Microsoft.EntityFrameworkCore.Migrations;

namespace FixIt_Data.Migrations
{
    public partial class minor_fix_in_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "longitude",
                table: "Issues",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "latitude",
                table: "Issues",
                newName: "Latitude");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Issues",
                newName: "longitude");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Issues",
                newName: "latitude");
        }
    }
}
