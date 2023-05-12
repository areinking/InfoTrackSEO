using InfoTrackSEO.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InfoTrackSEO.Domain.Repositories
{
    public class SearchResultRepository : ISearchResultRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SearchResultRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SearchResult?> GetByIdAsync(Guid id)
        {
            return await _dbContext.SearchResults.FindAsync(id);
        }

        public async Task<IEnumerable<SearchResult>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbContext.SearchResults
                .Where(sr => sr.SearchDate >= startDate && sr.SearchDate <= endDate)
                .ToListAsync();
        }

        public async Task<SearchResult> AddAsync(SearchResult searchResult)
        {
            var result = await _dbContext.SearchResults.AddAsync(searchResult);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<SearchResult> UpdateAsync(SearchResult searchResult)
        {
            _dbContext.Entry(searchResult).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return searchResult;
        }

        public async Task DeleteAsync(Guid id)
        {
            var searchResult = await _dbContext.SearchResults.FindAsync(id);
            if (searchResult != null)
            {
                _dbContext.SearchResults.Remove(searchResult);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
