using BLOG.Api.Context;
using BLOG.Api.DTOS;
using BLOG.Api.Errors;
using BLOG.Api.Models;
using BLOG.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BLOG.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IGenericRepository<Article> _articleRepo;
        private readonly ApplicationDbContext _dbContext;

        public ArticlesController(IGenericRepository<Article> articleRepo,
            ApplicationDbContext dbContext)
        {
            _articleRepo = articleRepo;
            _dbContext = dbContext;
        }
        // GET: api/Articles
        [HttpGet]
        public async Task<IActionResult> GetAllArticles()
        {
            var articles =await _articleRepo.GetAllAsync();
            var articleResponseDtos = articles.Select(article => new ArticleResponseDTO
            {
                Id = article.Id,
                Picture = article.Picture,
                AddressEn = article.AddressEn,
                AddressAr = article.AddressAr,
                ContentEn = article.ContentEn,
                ContentAr = article.ContentAr,
                Viewed = article.Viewed,
                CatIds = article.ArticleCategories.Select(ac => ac.CategoryId).ToArray() // تحويل إلى CatIds
            }).ToList();

            return Ok(articleResponseDtos);
        }
        //Details
        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var article = await _articleRepo.GetByIdAsync(id);
            if (article == null)
            {
                return NotFound(new ApiResponse(404));
            }
            article.Viewed += 1;
            _articleRepo.Update(article);
            var articleResponseDto = new ArticleResponseDTO
            {
                Id = article.Id,
                Picture = article.Picture,
                AddressEn = article.AddressEn,
                AddressAr = article.AddressAr,
                ContentEn = article.ContentEn,
                ContentAr = article.ContentAr,
                Viewed = article.Viewed,
                CatIds = article.ArticleCategories.Select(ac => ac.CategoryId).ToArray()
            };
            return CreatedAtAction("GetAllArticles", new { id = article.Id }, articleResponseDto);
        }
        //add
        // POST: api/Articles
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddArticle([FromForm] ArticleDTO articleDTO)
        {
            string picturePath = await SavePictureAsync(articleDTO.Picture);
            var validCategoryIds = await _dbContext.Categeries
        .Select(c => c.Id)
        .ToListAsync();

            
            var invalidCategoryIds = articleDTO.CatIds
                .Where(id => !validCategoryIds.Contains(id))
                .ToList();

            if (invalidCategoryIds.Any())
            {
                
                return BadRequest(new ApiResponse(400, "Invalid Category IDs provided."));
            }
            
            var article = new Article
            {
                Picture = picturePath,
                AddressEn = articleDTO.AddressEn,
                AddressAr = articleDTO.AddressAr,
                ContentEn = articleDTO.ContentEn,
                ContentAr = articleDTO.ContentAr,
                Viewed = 0, 
                ArticleCategories = articleDTO.CatIds.Select(id => new ArticleCategory { CategoryId = id }).ToList()
            };
            await _articleRepo.AddAsync(article);
            var articleResponseDto = new ArticleResponseDTO
            {
                Id = article.Id,
                Picture = article.Picture,
                AddressEn = article.AddressEn,
                AddressAr = article.AddressAr,
                ContentEn = article.ContentEn,
                ContentAr = article.ContentAr,
                Viewed = article.Viewed,
                CatIds = article.ArticleCategories.Select(ac => ac.CategoryId).ToArray()
            };
            return CreatedAtAction("GetAllArticles", new { id = article.Id }, articleResponseDto);
        }
        //delete
        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<bool>> DeleteArticle(int id)
        {
            var article = await _articleRepo.GetByIdAsync(id);
            if (article == null)
            {
                return NotFound(new ApiResponse(404));
            }
            var Result=_articleRepo.Delete(article);
            if (Result)
                return Ok(new { message = "Deleted successfully" });
            else
                return BadRequest(new { message = "An error occurred while deleting the Article." });
            
        }
        //update
        // PUT: api/Articles/
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateArticle(int id, [FromForm] ArticleDTO articleDTO)
        {
            var article = await _articleRepo.GetByIdAsync(id);
            if (article == null)
            {
                return NotFound(new ApiResponse(404));
            }

            string picturePath = article.Picture; 

           
            if (articleDTO.Picture != null && articleDTO.Picture.Length > 0)
            {
                
                var newFileName = Path.GetFileName(articleDTO.Picture.FileName);
                var currentFileName = Path.GetFileName(article.Picture);

                if (newFileName != currentFileName)
                {
                    
                    picturePath = await SavePictureAsync(articleDTO.Picture);
                }
            }
            
            article.Picture = picturePath; 
            article.AddressEn = articleDTO.AddressEn;
            article.AddressAr = articleDTO.AddressAr;
            article.ContentEn = articleDTO.ContentEn;
            article.ContentAr = articleDTO.ContentAr;
            article.ArticleCategories = articleDTO.CatIds.Select(id => new ArticleCategory { CategoryId = id }).ToList();

            
            _articleRepo.Update(article);

            
            var articleResponseDto = new ArticleResponseDTO
            {
                Id = article.Id,
                Picture = article.Picture,
                AddressEn = article.AddressEn,
                AddressAr = article.AddressAr,
                ContentEn = article.ContentEn,
                ContentAr = article.ContentAr,
                Viewed = article.Viewed,
                CatIds = article.ArticleCategories.Select(ac => ac.CategoryId).ToArray()
            };

            return Ok(articleResponseDto);
        }

        private async Task<string> SavePictureAsync(IFormFile picture)
        {
            if (picture == null || picture.Length == 0)
            {
                return null; 
            }

            var uploadsFolder = Path.Combine("Uploads", "Articles");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(picture.FileName);
            var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await picture.CopyToAsync(stream);
            }

            return $"/Uploads/Articles/{uniqueFileName}";
        }

    }


}
