
using BLOG.Api.Context;
using BLOG.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BLOG.Api.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(T item)
        {
            _dbContext.Set<T>().Add(item);
            await _dbContext.SaveChangesAsync();
        }

        public bool Delete(T item)
        {
            _dbContext.Remove(item);
            _dbContext.SaveChanges();
            return true;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        { 
            if(typeof(T) == typeof(Article))
            {
                return(IEnumerable<T>) await _dbContext.Articles
               .Include(a => a.ArticleCategories) 
                .ToListAsync();
            }
            return await _dbContext.Set<T>().ToListAsync();  
        }

        public async Task<T> GetByIdAsync(int id)
        {
            if (typeof(T) == typeof(Article))
            {
                return await _dbContext.Articles
               .Include(a => a.ArticleCategories)
                .FirstOrDefaultAsync(a => a.Id == id) as T;
            }
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public void Update(T item)
        {
            _dbContext.Update(item);
            _dbContext.SaveChanges();
        }
    }
}
