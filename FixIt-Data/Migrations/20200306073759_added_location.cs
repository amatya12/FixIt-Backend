using Microsoft.EntityFrameworkCore.Migrations;

namespace FixIt_Data.Migrations
{
    public partial class added_location : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Departments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Departments");
        }
    }
}
