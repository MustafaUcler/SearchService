using microservice_search_ads.Model;
using Microsoft.EntityFrameworkCore;

namespace microservice_search_ads
{
    public class SearchDbContext : DbContext
    {
        public DbSet<AdModel> AdModels { get; set; }

        public SearchDbContext(DbContextOptions<SearchDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
