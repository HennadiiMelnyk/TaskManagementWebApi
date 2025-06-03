using TaskManagementWebApi.Application.States.Interfaces;
using TaskManagementWebApi.Domain.Entities;

namespace TaskManagementWebApi.Application.States;

public class TaskStateContext
{
    private ITaskState _state;

    public TaskStateContext(ITaskState state)
    {
        _state = state;
    }

    public void SetState(ITaskState newState)
    {
        _state = newState;
    }

    public void Handle(TaskItem task)
    {
        _state.Handle(task);
    }

    public TaskState? TryTransition(TaskItem task)
    {
        return _state.TryTransition(task);
    }
}