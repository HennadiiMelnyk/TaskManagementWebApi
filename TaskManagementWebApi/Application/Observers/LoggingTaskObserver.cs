using TaskManagementWebApi.Application.Observers.Interfaces;
using TaskManagementWebApi.Domain.Entities;

namespace TaskManagementWebApi.Application.Observers;

public class LoggingTaskObserver : ITaskObserver
{
    private readonly ILogger<LoggingTaskObserver> _logger;

    public LoggingTaskObserver(ILogger<LoggingTaskObserver> logger)
    {
        _logger = logger;
    }

    public Task OnTaskChangedAsync(TaskItem task, TaskChangeContext context, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Task {TaskId} changed: State {OldState} → {NewState}, User {OldUser} → {NewUser}",
            task.Id,
            context.PreviousState,
            task.State,
            context.PreviousUserId,
            task.AssignedUserId
        );

        return Task.CompletedTask;
    }
}
