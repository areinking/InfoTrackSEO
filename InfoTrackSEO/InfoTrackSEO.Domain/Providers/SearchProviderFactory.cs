using Microsoft.Extensions.DependencyInjection;

public class SearchProviderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public SearchProviderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ISearchProvider GetSearchProvider(string searchProviderName)
    {
        ISearchProvider? searchProvider;

        switch (searchProviderName.ToLower())
        {
            case "google":
                searchProvider = _serviceProvider.GetService<GoogleSearchProvider>();
                break;
            default: 
                throw new ArgumentException("Invalid search engine provided", searchProviderName);
        };

        if (searchProvider == null) {
            throw new Exception($"Cannot find the implementation for {searchProvider}");
        }

        return searchProvider;
    }
}