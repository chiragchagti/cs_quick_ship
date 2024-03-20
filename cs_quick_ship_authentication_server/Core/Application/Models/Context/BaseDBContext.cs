using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Application.Models.Context
{
    public class BaseDBContext : IdentityDbContext<AppUser>
    {
        public BaseDBContext(DbContextOptions<BaseDBContext> options) : base(options)
        {

        }
        public DbSet<OAuthApplicationEntity> OAuthApplications { get; set; }
        public DbSet<OAuthTokenEntity> OAuthTokens { get; set; }
    }
}
