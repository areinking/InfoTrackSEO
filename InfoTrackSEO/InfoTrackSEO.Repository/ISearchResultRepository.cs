using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfoTrackSEO.Domain.Models;

namespace InfoTrackSEO.Repository
{
    public interface ISearchResultRepository
    {
        Task<IEnumerable<SearchResult>> GetRangeAsync(DateTime startDate, DateTime endDate);
        Task AddAsync(SearchResult searchResult);
    }
}
