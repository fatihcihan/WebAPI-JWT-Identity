using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAPI_JWT_Identity.Models;

namespace WebAPI_JWT_Identity.Context
{
    public class ApiDbContext : IdentityDbContext<ApplicationUser>
    {
        public readonly IHttpContextAccessor _httpContextAccessor;
        public ApiDbContext(DbContextOptions<ApiDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<ApplicationUserTokens> ApplicationUserTokens { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Product>().ToTable("Products");
            builder.Entity<Product>().HasIndex(x => x.Name).IsUnique();
        }
    }
}
