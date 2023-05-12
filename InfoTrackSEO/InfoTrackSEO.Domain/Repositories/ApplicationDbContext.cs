using InfoTrackSEO.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InfoTrackSEO.Domain
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
        
        #nullable disable
        public DbSet<SearchResult> SearchResults { get; set; }
        #nullable restore
    }
}
