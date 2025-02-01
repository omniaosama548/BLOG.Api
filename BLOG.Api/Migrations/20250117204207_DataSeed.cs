using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BLOG.Api.Migrations
{
    /// <inheritdoc />
    public partial class DataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "AddressAr", "AddressEn", "ContentAr", "ContentEn", "Picture", "Viewed" },
                values: new object[,]
                {
                    { 1, "أخبار التكنولوجيا", "Tech News", "أحدث التحديثات في عالم التكنولوجيا.", "Latest updates in the technology world.", "tech_image.jpg", 0 },
                    { 2, "نصائح صحية", "Health Tips", "كيف تبقى بصحة جيدة في العالم الحديث.", "How to stay healthy in the modern world.", "health_image.jpg", 0 }
                });

            migrationBuilder.InsertData(
                table: "Categeries",
                columns: new[] { "Id", "NameAr", "NameEn" },
                values: new object[,]
                {
                    { 1, "تقنية", "Technology" },
                    { 2, "صحة", "Health" },
                    { 3, "رياضة", "Sports" }
                });

            migrationBuilder.InsertData(
                table: "ArticleCategories",
                columns: new[] { "ArticleId", "CategoryId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ArticleCategories",
                keyColumns: new[] { "ArticleId", "CategoryId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "ArticleCategories",
                keyColumns: new[] { "ArticleId", "CategoryId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "Categeries",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Articles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categeries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categeries",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
