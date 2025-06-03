using System.Text.Json.Serialization;
using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
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
    public static IHostBuilder AddHost(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureWebHost(builder =>
        {
            builder.UseKestrel();

            builder.ConfigureServices(services =>
            {
                services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    });
            });

            builder.Configure((context, app) =>
            {
                var isDevelopmentEnv = context.HostingEnvironment.IsStaging() || context.HostingEnvironment.IsDevelopment();

                // app.UseMiddleware<ExceptionHandlingMiddleware>();
                // app.UseHeaderPropagation();
                //
                // app.UseSwagger();
                // app.UseSwaggerUI(c =>
                // {
                //     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task management web API v1");
                // });


                app.UseRouting();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            });
        });

        return hostBuilder;
    }
    
    public static IServiceCollection AddSwaggerWithApiVersioning(this IServiceCollection services)
    {
        services
            .AddEndpointsApiExplorer()
            .AddApiVersioning(config =>
            {
                config.DefaultApiVersion = ApiVersion.Default;
                config.ReportApiVersions = true;
                config.AssumeDefaultVersionWhenUnspecified = true;
            })
            .AddMvc()
            .AddApiExplorer(options => options.GroupNameFormat = "'v'VVV");

        // services.AddSwagger(versioningEnabled: true);

        return services;
    }
    
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
        
        services.AddSingleton<TaskEventPublisher>();
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
}