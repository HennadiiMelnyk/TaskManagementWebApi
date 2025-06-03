using TaskManagementWebApi.Domain.Entities;

namespace TaskManagementWebApi.Infrastructure.Repositories.Interfaces;

public interface ITaskAssignmentHistoryRepository
{
    Task AddAsync(TaskAssignmentHistory history, CancellationToken token);
    
    Task<TaskAssignmentHistory[]> GetHistoryForTaskAsync(int taskId, CancellationToken token);
}