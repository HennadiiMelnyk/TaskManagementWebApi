using Microsoft.EntityFrameworkCore;
using TaskManagementWebApi.Domain.Entities;
using TaskManagementWebApi.Infrastructure.Persistence;
using TaskManagementWebApi.Infrastructure.Repositories.Interfaces;

namespace TaskManagementWebApi.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TaskManagementDbContext _context;

    public TaskRepository(TaskManagementDbContext context) => _context = context;

    public async Task<TaskItem> AddAsync(TaskItem task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public Task<TaskItem?> GetByTitleAsync(string title) =>
        _context.Tasks.FirstOrDefaultAsync(t => t.Title == title);

    public Task<TaskItem[]> GetAllAsync() =>
        _context.Tasks.AsNoTracking().ToArrayAsync();
}