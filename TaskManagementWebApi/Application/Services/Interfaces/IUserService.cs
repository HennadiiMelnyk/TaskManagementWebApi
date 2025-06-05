using TaskManagementWebApi.Application.DTO;

namespace TaskManagementWebApi.Application.Services.Interfaces;

public interface IUserService
{
    Task<UserDto[]> GetAllAsync();

    Task<UserDto> CreateAsync(UserCreateDto dto);
}