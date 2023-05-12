using InfoTrackSEO.Domain.Models;

namespace InfoTrackSEO.Domain.Repositories
{
    public interface ISearchResultRepository
    {
        Task<SearchResult?> GetByIdAsync(Guid id);
        Task<IEnumerable<SearchResult>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<SearchResult> AddAsync(SearchResult searchResult);
        Task<SearchResult> UpdateAsync(SearchResult searchResult);
        Task DeleteAsync(Guid id);
    }
}