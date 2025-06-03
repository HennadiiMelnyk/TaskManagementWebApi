using TaskManagementWebApi.Application.States;
using TaskManagementWebApi.Application.States.Interfaces;
using TaskManagementWebApi.Domain.Entities;

namespace TaskManagementWebApi.Application.Services;

public static class TaskStateFactory
{
    public static ITaskState Create(TaskState state) => state switch
    {
        TaskState.Waiting => new WaitingState(),
        TaskState.InProgress => new InProgressState(),
        TaskState.Completed => new CompletedState(),
        _ => throw new ArgumentOutOfRangeException(nameof(state), $"Unknown state: {state}")
    };
}