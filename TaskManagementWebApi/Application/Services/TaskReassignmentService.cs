namespace TaskManagementWebApi.Application.Services;

public class TaskReassignmentService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory; 
    
    private readonly ILogger<TaskReassignmentService> _logger;
    
    public TaskReassignmentService(IServiceScopeFactory scopeFactory, ILogger<TaskReassignmentService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        throw new NotImplementedException();
    }
}