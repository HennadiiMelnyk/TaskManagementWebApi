using TaskManagementWebApi.Domain.Entities;

namespace TaskManagementWebApi.Infrastructure.Repositories.Interfaces;

public interface ITaskRepository
{
    Task<TaskItem> AddAsync(TaskItem task);
    
    Task<TaskItem?> GetByTitleAsync(string title);
    
    Task<TaskItem[]> GetAllAsync();
}