using TaskManagementWebApi.Application.DTO;
using TaskManagementWebApi.Application.Services.Interfaces;
using TaskManagementWebApi.Domain.Entities;
using TaskManagementWebApi.Infrastructure.Repositories.Interfaces;

namespace TaskManagementWebApi.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepo;
    private readonly IUserRepository _userRepo;

    public TaskService(ITaskRepository taskRepo, IUserRepository userRepo)
    {
        _taskRepo = taskRepo;
        _userRepo = userRepo;
    }

    public async Task<TaskDto> CreateAsync(TaskCreateDto dto)
    {
        var existing = await _taskRepo.GetByTitleAsync(dto.Title);
        if (existing != null)
            throw new InvalidOperationException("Task already exists");

        var users = await _userRepo.GetAllAsync();
        User? assigned = null;

        if (users.Length > 0)
        {
            var rnd = new Random();
            assigned = users[rnd.Next(users.Length)];
        }

        var task = new TaskItem
        {
            Title = dto.Title,
            AssignedUserId = assigned?.Id,
            State = assigned != null ? TaskState.InProgress : TaskState.Waiting
        };

        var result = await _taskRepo.AddAsync(task);

        return new TaskDto
        {
            Id = result.Id,
            Title = result.Title,
            AssignedUserId = result.AssignedUserId,
            State = result.State.ToString()
        };
    }

    public async Task<TaskDto[]> GetAllAsync()
    {
        var tasks = await _taskRepo.GetAllAsync();
        return tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            State = t.State.ToString(),
            AssignedUserId = t.AssignedUserId
        }).ToArray();
    }
}