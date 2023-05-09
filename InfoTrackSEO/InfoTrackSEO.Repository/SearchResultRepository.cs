using InfoTrackSEO.Domain.Models;
using MongoDB.Driver;

namespace InfoTrackSEO.Repository
{
    public class SearchResultRepository : ISearchResultRepository
    {
        private readonly IMongoCollection<SearchResult> _searchResults;

        public SearchResultRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("InfoTrackSEO");
            _searchResults = database.GetCollection<SearchResult>("SearchResults");
        }

        public async Task<IEnumerable<SearchResult>> GetRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _searchResults
                .Find(searchResult => searchResult.SearchDate >= startDate && searchResult.SearchDate <= endDate)
                .ToListAsync();
        }

        public async Task AddAsync(SearchResult searchResult)
        {
            await _searchResults.InsertOneAsync(searchResult);
        }
    }
}
