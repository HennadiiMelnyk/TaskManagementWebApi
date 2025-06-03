using TaskManagementWebApi.Application.States.Interfaces;
using TaskManagementWebApi.Domain.Entities;

namespace TaskManagementWebApi.Application.States;

public class CompletedState : ITaskState
{
    public TaskState State => TaskState.Completed;

    public void Handle(TaskItem task)
    {
        task.State = TaskState.Completed;
        task.AssignedUserId = null;
    }

    public TaskState? TryTransition(TaskItem task) => null;
    
}