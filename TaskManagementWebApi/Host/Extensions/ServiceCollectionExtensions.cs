using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;
using TaskManagementWebApi.Application.Handlers;
using TaskManagementWebApi.Application.Handlers.Interfaces;
using TaskManagementWebApi.Application.Observers;
using TaskManagementWebApi.Application.Observers.Interfaces;
using TaskManagementWebApi.Application.Services;
using TaskManagementWebApi.Application.Services.Interfaces;
using TaskManagementWebApi.Infrastructure.Persistence;
using TaskManagementWebApi.Infrastructure.Repositories;
using TaskManagementWebApi.Infrastructure.Repositories.Interfaces;

namespace TaskManagementWebApi.Host.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<ITaskRepository, TaskRepository>();
        serviceCollection.AddScoped<ITaskAssignmentHistoryRepository, TaskAssignmentHistoryRepository>();

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DefaultConnection"));
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        serviceCollection.AddDbContext<TaskManagementDbContext>(options =>
        {
            options.UseNpgsql(dataSource);
        });

        return serviceCollection;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IUserService, UserService>();
        services.AddHostedService<TaskReassignmentService>();
        
        services.AddTransient<ICandidateSelectionHandler, ExcludeCurrentUserHandler>();
        services.AddTransient<ICandidateSelectionHandler, ExcludePreviousUserHandler>();
        services.AddSingleton<CandidateHandlerChainBuilder>();
        
        services.AddScoped<TaskEventPublisher>();
        services.AddScoped<ITaskObserver, LoggingTaskObserver>();

        return services;
    }
    
    public static IHost MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        using var appContext = scope.ServiceProvider.GetRequiredService<TaskManagementDbContext>();

        appContext.Database.Migrate();

        return host;
    }
    
    public static IServiceCollection AddSwaggerWithApiVersioning(this IServiceCollection services)
    {
        services
            .AddEndpointsApiExplorer()
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Task Management API",
                Description = "API for managing users and tasks"
            });

            options.UseInlineDefinitionsForEnums();
        });

        return services;
    }
}