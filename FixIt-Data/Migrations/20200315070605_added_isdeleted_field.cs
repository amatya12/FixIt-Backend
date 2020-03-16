using Microsoft.EntityFrameworkCore.Migrations;

namespace FixIt_Data.Migrations
{
    public partial class added_isdeleted_field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Issues",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Issues");
        }
    }
}
