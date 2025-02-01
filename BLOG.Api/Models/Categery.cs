namespace BLOG.Api.Models
{
    public class Categery
    {
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        // Navigation property for Many-to-Many relationship
        public ICollection<ArticleCategory> ArticleCategories { get; set; } = new List<ArticleCategory>();
    }
}
