using InfoTrackSEO.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

// need this for migrations
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    ApplicationDbContext IDesignTimeDbContextFactory<ApplicationDbContext>.CreateDbContext(
        string[] args
    )
    {
        IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile(
            "appsettings.json",
            false,
            true
        );
        IConfigurationRoot configuration = builder.Build();

        var dbBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        dbBuilder.UseSqlServer(connectionString);

        return new ApplicationDbContext(dbBuilder.Options);
    }
}
