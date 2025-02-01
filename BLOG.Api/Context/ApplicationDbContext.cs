using BLOG.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BLOG.Api.Context
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ArticleCategory>()
                .HasKey(ac => new { ac.ArticleId, ac.CategoryId });

            modelBuilder.Entity<Categery>().HasData(
         new Categery { Id = 1, NameAr = "تقنية", NameEn = "Technology" },
         new Categery { Id = 2, NameAr = "صحة", NameEn = "Health" },
         new Categery { Id = 3, NameAr = "رياضة", NameEn = "Sports" }
     );

            // Seeding Articles
            modelBuilder.Entity<Article>().HasData(
                new Article
                {
                    Id = 1,
                    AddressEn = "Tech News",
                    AddressAr = "أخبار التكنولوجيا",
                    ContentEn = "Latest updates in the technology world.",
                    ContentAr = "أحدث التحديثات في عالم التكنولوجيا.",
                    Viewed = 0,
                    Picture = "tech_image.jpg"
                },
                new Article
                {
                    Id = 2,
                    AddressEn = "Health Tips",
                    AddressAr = "نصائح صحية",
                    ContentEn = "How to stay healthy in the modern world.",
                    ContentAr = "كيف تبقى بصحة جيدة في العالم الحديث.",
                    Viewed = 0,
                    Picture = "health_image.jpg"
                }
            );
            modelBuilder.Entity<ArticleCategory>().HasData(
            new ArticleCategory { ArticleId = 1, CategoryId = 1 },
            new ArticleCategory { ArticleId = 2, CategoryId = 2 }
        );
            modelBuilder.Entity<Admin>().HasData(
           new Admin
           {
               Id = 1,
               UserName = "omnia",
               Password = "12345" // هنا استخدمي كلمة مرور مشفرة
           }
       );
        }
        public DbSet<Models.Article> Articles { get; set; }
        public DbSet<Models.Categery> Categeries { get; set; }
        public DbSet<Models.ArticleCategory> ArticleCategories { get; set; }
        public DbSet<Admin>Admins { get; set; }
}
    
}
