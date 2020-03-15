using Microsoft.EntityFrameworkCore.Migrations;

namespace FixIt_Data.Migrations
{
    public partial class Fixed_minor_datatype_issue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Issues",
                nullable: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Issues",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
