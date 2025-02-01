using BLOG.Api.Migrations;
using BLOG.Api.Models;
using Admin = BLOG.Api.Models.Admin;

namespace BLOG.Api.Services
{
    public interface ITokenServices
    {
        Task<string> CreateTokenAsync(Admin admin);
    }
}
