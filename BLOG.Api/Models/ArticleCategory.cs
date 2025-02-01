namespace BLOG.Api.Models
{
    public class ArticleCategory
    {
        public int ArticleId { get; set; }
        public Article Article { get; set; }

        public int CategoryId { get; set; }
        public Categery Category { get; set; }
    }
}
