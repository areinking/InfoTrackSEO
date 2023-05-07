using System.Threading.Tasks;
using InfoTrackSEO.Domain.Models;

namespace InfoTrackSEO.Domain.DomainServices
{
    public interface ISearchService
    {
        Task<SearchResult> SearchAsync(string keywords, string url, string searchEngine);
    }
}
