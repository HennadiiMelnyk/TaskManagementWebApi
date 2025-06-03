using TaskManagementWebApi.Application.States.Interfaces;
using TaskManagementWebApi.Domain.Entities;

namespace TaskManagementWebApi.Application.States;

public class InProgressState : ITaskState
{
    public TaskState State => TaskState.InProgress;

    public void Handle(TaskItem task)
    {
        task.State = TaskState.InProgress;
    }

    public TaskState? TryTransition(TaskItem task)
    {
        return null;
    }
}