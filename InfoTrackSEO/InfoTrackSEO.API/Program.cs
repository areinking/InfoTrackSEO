using InfoTrackSEO.Domain;
using InfoTrackSEO.Domain.EventBus;
using InfoTrackSEO.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddCors(p => p.AddPolicy("cors", builder =>
        {
            builder.WithOrigins("http://localhost:54321", "https://localhost:54321", "http://[::1]:54321", "https://[::1]:54321")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .SetIsOriginAllowedToAllowWildcardSubdomains()
                   .AllowCredentials();
        }));

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        // Register search services
        builder.Services.AddScoped<GoogleSearchProvider>();
        builder.Services.AddScoped<SearchProviderFactory>();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<ISearchResultRepository, SearchResultRepository>();
        builder.Services.AddScoped<IEventBus, EventBus>();
        builder.Services.AddHttpClient();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("cors");
        app.MapControllers();

        app.Run();
    }
}