using InfoTrackSEO.Repository;
using MongoDB.Driver;

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
builder.Services.AddTransient<GoogleSearchProvider>();
builder.Services.AddSingleton<SearchProviderFactory>();

// Register MongoDB
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDb");
    return new MongoClient(connectionString);
});

builder.Services.AddScoped<ISearchResultRepository, SearchResultRepository>();

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
