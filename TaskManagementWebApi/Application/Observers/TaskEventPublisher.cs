using TaskManagementWebApi.Application.Observers.Interfaces;
using TaskManagementWebApi.Domain.Entities;

namespace TaskManagementWebApi.Application.Observers;

public class TaskEventPublisher
{
    private readonly IEnumerable<ITaskObserver> _observers;

    public TaskEventPublisher(IEnumerable<ITaskObserver> observers)
    {
        _observers = observers;
    }

    public async Task PublishAsync(TaskItem task, TaskChangeContext context, CancellationToken token)
    {
        foreach (var observer in _observers)
        {
            await observer.OnTaskChangedAsync(task, context, token);
        }
    }
}
