using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskManagementWebApi.Infrastructure.Persistence;

public class TaskManagementDbContextFactory : IDesignTimeDbContextFactory<TaskManagementDbContext>
{
    public TaskManagementDbContext CreateDbContext(string[] args)
    {
        // Build configuration
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Create options builder using Npgsql
        var builder = new DbContextOptionsBuilder<TaskManagementDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        builder.UseNpgsql(connectionString);

        return new TaskManagementDbContext(builder.Options);
    }
}