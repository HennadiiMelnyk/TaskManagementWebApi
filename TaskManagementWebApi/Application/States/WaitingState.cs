using TaskManagementWebApi.Application.States.Interfaces;
using TaskManagementWebApi.Domain.Entities;

namespace TaskManagementWebApi.Application.States;

public class WaitingState : ITaskState
{
    public TaskState State => TaskState.Waiting;

    public void Handle(TaskItem task)
    {
        task.State = TaskState.Waiting;
        task.AssignedUserId = null;
    }

    public TaskState? TryTransition(TaskItem task)
    {
        return task.AssignedUserId != null ? TaskState.InProgress : null;
    }
}