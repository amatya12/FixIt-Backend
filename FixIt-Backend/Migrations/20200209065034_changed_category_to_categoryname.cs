using Microsoft.EntityFrameworkCore.Migrations;

namespace FixIt_Backend.Migrations
{
    public partial class changed_category_to_categoryname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Categories",
                table: "Categories",
                newName: "CategoryName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "Categories",
                newName: "Categories");
        }
    }
}
