namespace BLOG.Api.DTOS
{
    public class ArticleResponseDTO
    {
        public int Id { get; set; }
        public string Picture { get; set; }
        public string AddressEn { get; set; }
        public string AddressAr { get; set; }
        public string ContentEn { get; set; }
        public string ContentAr { get; set; }
        public int Viewed { get; set; } // يتم عرضه للإشارة إلى عدد المشاهدات
        public int[] CatIds { get; set; }
    }
}
