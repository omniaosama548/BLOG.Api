using BLOG.Api.Context;
using BLOG.Api.DTOS;
using BLOG.Api.Errors;
using BLOG.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BLOG.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ITokenServices _tokenServices;

        public AdminsController(ApplicationDbContext dbContext,ITokenServices tokenServices)
        {
            _dbContext = dbContext;
            _tokenServices = tokenServices;
        }
        //login
        [HttpPost("login")]
        public async Task<ActionResult>Login([FromForm]AdminLoginDTO model)
        {
            var admin =  _dbContext.Admins.FirstOrDefault(a => a.UserName == model.UserName );
            if (admin == null || admin.Password!= model.Password)
            {
                return BadRequest(new ApiResponse(401, "Invalid username or password."));
            }
            var token = await _tokenServices.CreateTokenAsync(admin);
            return Ok(new { username=admin.UserName,password=admin.Password, token = token });
        }
    }
}
