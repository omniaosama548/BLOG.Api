namespace BLOG.Api.DTOS
{
    public class ArticleDTO
    {
        
        public string AddressEn { get; set; } 
        public string AddressAr { get; set; } 
        public string ContentEn { get; set; } 
        public string ContentAr { get; set; }
        public IFormFile Picture { get; set; }
        
      
        public int[] CatIds { get; set; }
    }
}
