namespace BLOG.Api.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Picture { get; set; } 
        public string AddressEn { get; set; }
        public string AddressAr { get; set; }
        public string ContentEn { get; set; }
        public string ContentAr { get; set; }
        public int Viewed { get; set; }
        public ICollection<ArticleCategory> ArticleCategories { get; set; }= new List<ArticleCategory>();
    }
}
