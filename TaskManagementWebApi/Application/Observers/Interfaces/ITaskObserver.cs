using TaskManagementWebApi.Domain.Entities;

namespace TaskManagementWebApi.Application.Observers.Interfaces;

public interface ITaskObserver
{
    Task OnTaskChangedAsync(TaskItem task, TaskChangeContext context, CancellationToken cancellationToken);
}
