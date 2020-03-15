using Microsoft.EntityFrameworkCore.Migrations;

namespace FixIt_Data.Migrations
{
    public partial class refactored_issue_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Issues");

            migrationBuilder.RenameColumn(
                name: "Severity",
                table: "Issues",
                newName: "Priority");

            migrationBuilder.RenameColumn(
                name: "ExtraInformation",
                table: "Issues",
                newName: "Location");

            migrationBuilder.AddColumn<string>(
                name: "DateCreated",
                table: "Issues",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Issues",
                table: "Issues",
                type: "varchar(1000)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "Issues",
                table: "Issues");

            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "Issues",
                newName: "Severity");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Issues",
                newName: "ExtraInformation");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Issues",
                type: "varchar(5000)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Issues",
                type: "varchar(500)",
                nullable: true);
        }
    }
}
