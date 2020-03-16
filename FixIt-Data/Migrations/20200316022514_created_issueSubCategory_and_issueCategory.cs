using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FixIt_Data.Migrations
{
    public partial class created_issueSubCategory_and_issueCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Categories_CategoryId",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_CategoryId",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Issues");

            migrationBuilder.CreateTable(
                name: "IssueCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IssueId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssueCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IssueCategories_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IssueSubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IssueId = table.Column<int>(nullable: false),
                    SubCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueSubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssueSubCategories_Issues_IssueId",
                        column: x => x.IssueId,
                        principalTable: "Issues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IssueSubCategories_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IssueCategories_CategoryId",
                table: "IssueCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_IssueCategories_IssueId",
                table: "IssueCategories",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_IssueSubCategories_IssueId",
                table: "IssueSubCategories",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_IssueSubCategories_SubCategoryId",
                table: "IssueSubCategories",
                column: "SubCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssueCategories");

            migrationBuilder.DropTable(
                name: "IssueSubCategories");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Issues",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Issues_CategoryId",
                table: "Issues",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Categories_CategoryId",
                table: "Issues",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
