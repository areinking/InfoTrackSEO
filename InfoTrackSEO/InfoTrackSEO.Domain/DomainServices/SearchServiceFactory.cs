using Microsoft.Extensions.DependencyInjection;

namespace InfoTrackSEO.Domain.DomainServices
{
    public class SearchServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public SearchServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ISearchService CreateSearchService(string searchEngine)
        {
            ISearchService? service;

            switch (searchEngine.ToLower())
            {
                case "google": 
                    service = _serviceProvider.GetService<GoogleSearchService>();
                    break;
                case "bing": 
                    service = _serviceProvider.GetService<BingSearchService>();
                    break;
                default: 
                    throw new ArgumentException("Invalid search engine provided", nameof(searchEngine));
            };

            if (service == null) {
                throw new Exception($"Cannot find the implementation for {searchEngine}");
            }

            return service;
        }
    }
}
