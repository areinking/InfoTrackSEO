using InfoTrackSEO.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InfoTrackSEO.Domain.Repositories
{
    public class SearchResultRepository : ISearchResultRepository
    {
        private readonly ApplicationDbContext _dbContext;

        private async Task AddChildrenLinkPositions(IEnumerable<LinkPosition> linkPositions)
        {
            if (!linkPositions.Any())
            {
                return;
            }

            // We only add the results once and do not update, so no reason to update
            foreach (var linkPosition in linkPositions)
            {
                if (!_dbContext.LinkPositions.Any(l => l.Id == linkPosition.Id))
                {
                    await _dbContext.AddAsync(linkPosition);
                }
            }
            ;
        }

        public SearchResultRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SearchResult?> GetByIdAsync(Guid id)
        {
            return await _dbContext.SearchResults.FindAsync(id);
        }

        public IEnumerable<SearchResult> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            return _dbContext.SearchResults
                .Include(s => s.Results)
                .Where(sr => sr.SearchDate >= startDate && sr.SearchDate <= endDate)
                .ToList();
        }

        public async Task<SearchResult> AddAsync(SearchResult searchResult)
        {
            await AddChildrenLinkPositions(searchResult.Results);
            var result = await _dbContext.SearchResults.AddAsync(searchResult);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<SearchResult> UpdateAsync(SearchResult searchResult)
        {
            await AddChildrenLinkPositions(searchResult.Results);
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
