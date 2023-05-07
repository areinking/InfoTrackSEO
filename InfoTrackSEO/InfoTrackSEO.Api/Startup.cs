using InfoTrackSEO.API.Repositories;
using InfoTrackSEO.Domain.DomainServices;
using MongoDB.Driver;

namespace InfoTrackSEO.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Add CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod());
            });

            // Register search services
            services.AddTransient<GoogleSearchService>();
            services.AddTransient<BingSearchService>();
            services.AddSingleton<SearchServiceFactory>();

            // Register MongoDB
            services.AddSingleton<IMongoClient>(sp =>
            {
                var connectionString = Configuration.GetConnectionString("MongoDb");
                return new MongoClient(connectionString);
            });

            services.AddScoped<ISearchResultRepository, SearchResultRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Use CORS policy
            app.UseCors("AllowSpecificOrigin");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Configure static files
            app.UseStaticFiles();
        }
    }
}
