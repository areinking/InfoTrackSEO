using InfoTrackSEO.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InfoTrackSEO.Domain
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<SearchResult>()
                .HasMany(s => s.Results)
                .WithOne(l => l.SearchResult)
                .HasForeignKey(l => l.SearchResultId);
        }

#nullable disable
        public DbSet<SearchResult> SearchResults { get; set; }

        public DbSet<LinkPosition> LinkPositions { get; set; }

#nullable restore
    }
}
