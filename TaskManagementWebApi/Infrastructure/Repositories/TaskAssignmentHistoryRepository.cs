using Microsoft.EntityFrameworkCore;
using TaskManagementWebApi.Domain.Entities;
using TaskManagementWebApi.Infrastructure.Persistence;
using TaskManagementWebApi.Infrastructure.Repositories.Interfaces;

namespace TaskManagementWebApi.Infrastructure.Repositories;

public class TaskAssignmentHistoryRepository : ITaskAssignmentHistoryRepository
{
    private readonly TaskManagementDbContext _context;

    public TaskAssignmentHistoryRepository(TaskManagementDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TaskAssignmentHistory history, CancellationToken token)
    {
        await _context.TaskAssignments.AddAsync(history, token);
    }

    public async Task<TaskAssignmentHistory[]> GetHistoryForTaskAsync(int taskId, CancellationToken token)
    {
        return await _context.TaskAssignments
            .Where(h => h.TaskItemId == taskId)
            .OrderByDescending(h => h.AssignedAt)
            .ToArrayAsync(token);
    }
}