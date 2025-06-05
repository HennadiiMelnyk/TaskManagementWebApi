using System.Text.Json.Serialization;
using TaskManagementWebApi.Exceptions;
using TaskManagementWebApi.Host.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRepositories(builder.Configuration)
    .AddServices()
    .AddSwaggerWithApiVersioning();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Management API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MigrateDatabase();

app.Run();