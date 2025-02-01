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
    public class CategeriesController : ControllerBase
    {
        private readonly IGenericRepository<Categery> _categeryRepo;
        private readonly ApplicationDbContext _dbContext;
        private readonly IGenericRepository<Article> _articleRepo;

        public CategeriesController(IGenericRepository<Categery> categeryRepo,
            ApplicationDbContext dbContext,
            IGenericRepository<Article> articleRepo)
        {
            _categeryRepo = categeryRepo;
            _dbContext = dbContext;
            _articleRepo = articleRepo;
        }
        //add new categery
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddCategory([FromForm] CatogeryDTO categoryDTO)
        {

            if (string.IsNullOrEmpty(categoryDTO.NameEn) || string.IsNullOrEmpty(categoryDTO.NameAr))
            {
                return BadRequest(new ApiResponse(400, "Both English and Arabic names are required."));
            }
            var isCategoryExists = await _dbContext.Categeries
        .AnyAsync(c => c.NameEn.ToLower() == categoryDTO.NameEn.ToLower() ||
                       c.NameAr.ToLower() == categoryDTO.NameAr.ToLower());

            if (isCategoryExists)
            {
                return BadRequest(new ApiResponse(400, "Category with the same name already exists."));
            }
            var category = new Categery
            {
                NameEn = categoryDTO.NameEn,
                NameAr = categoryDTO.NameAr
            };

            await _categeryRepo.AddAsync(category);

            var response = new
            {
                id = category.Id,
                nameEn = category.NameEn,
                nameAr = category.NameAr
            };

            return Ok(response);
        }
        // GET: api/Categeries
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetAllCategeries()
        {
            var categories = await _dbContext.Categeries
        .Include(c => c.ArticleCategories) 
        .Select(c => new
        {
            id = c.Id,
            nameEn = c.NameEn,
            nameAr = c.NameAr,
            articleCategories = c.ArticleCategories.Select(ac => ac.ArticleId).ToList() // عرض فقط الـ IDs
        })
        .ToListAsync();

            return Ok(categories);
        }
        //Details
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<ActionResult> Details(int id)
        {
            var category = await _dbContext.Categeries
        .Include(c => c.ArticleCategories) // 
        .ThenInclude(ac => ac.Article)     
        .Where(c => c.Id == id)
        .Select(c => new
        {
            id = c.Id,
            nameEn = c.NameEn,
            nameAr = c.NameAr,
            articleCategories = c.ArticleCategories.Select(ac => new
            {
                articleId = ac.ArticleId,
                articleTitle = ac.Article.AddressEn 
            }).ToList()
        })
        .FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound(new ApiResponse(404, "Category not found"));
            }

            return Ok(category);
        }
        //Update
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateCategery(int id, [FromForm] CatogeryDTO categoryDTO)
        {
            var Categery = await _categeryRepo.GetByIdAsync(id);
            if (Categery == null)
            {
                return NotFound(new ApiResponse(404));
            }
            Categery.NameEn = categoryDTO.NameEn;
            Categery.NameAr = categoryDTO.NameAr;
            _categeryRepo.Update(Categery);
            var response = new
            {
                id = Categery.Id,
                nameEn =Categery.NameEn,
                nameAr = Categery.NameAr
            };

            return Ok(response);
        }
        //Delete
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var category = await _dbContext.Categeries.FindAsync(id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            var articles = await _dbContext.Articles
                .Where(a => a.ArticleCategories.Any(ac => ac.CategoryId == id))
                .Include(a => a.ArticleCategories) 
                .ToListAsync();

            foreach (var article in articles)
            {
                
                var articleCategoriesToRemove = article.ArticleCategories
                    .Where(ac => ac.CategoryId == id)
                    .ToList();

                foreach (var ac in articleCategoriesToRemove)
                {
                    article.ArticleCategories.Remove(ac);
                }

                
                if (!article.ArticleCategories.Any())
                {
                    _dbContext.Articles.Remove(article);
                }
                else
                {
                    _dbContext.Articles.Update(article); 
                }
            }

            _dbContext.Categeries.Remove(category);

            
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Deleted successfully" });
        }

       
    }
}
