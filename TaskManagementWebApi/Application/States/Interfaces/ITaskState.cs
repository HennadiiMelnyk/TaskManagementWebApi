using TaskManagementWebApi.Domain.Entities;

namespace TaskManagementWebApi.Application.States.Interfaces;

public interface ITaskState
{
    TaskState State { get; }
    
    void Handle(TaskItem task);
    
    TaskState? TryTransition(TaskItem task);
}