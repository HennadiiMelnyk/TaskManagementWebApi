using TaskManagementWebApi.Application.DTO;

namespace TaskManagementWebApi.Application.Services.Interfaces;

public interface ITaskService
{
    Task<TaskDto> CreateAsync(TaskCreateDto dto);
    Task<TaskDto[]> GetAllAsync();
}