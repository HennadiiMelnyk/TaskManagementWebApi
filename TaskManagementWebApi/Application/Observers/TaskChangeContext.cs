using TaskManagementWebApi.Domain.Entities;

namespace TaskManagementWebApi.Application.Observers;

public class TaskChangeContext
{
    public TaskState? PreviousState { get; set; }
    public int? PreviousUserId { get; set; }
}