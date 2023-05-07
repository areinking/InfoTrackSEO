using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfoTrackSEO.Domain.Models;

namespace InfoTrackSEO.API.Repositories
{
    public interface ISearchResultRepository
    {
        Task<IEnumerable<SearchResult>> GetAllAsync(DateTime startDate, DateTime endDate);
        Task AddAsync(SearchResult searchResult);
    }
}
