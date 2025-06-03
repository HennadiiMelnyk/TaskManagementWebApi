using TaskManagementWebApi.Application.DTO;

namespace TaskManagementWebApi.Application.Services.Interfaces;

public interface IUserService
{
    Task<User> AddAsync(User user);

    Task<User?> GetByNameAsync(string name);
    
    Task<UserDto[]> GetAllAsync();

    Task<UserDto> CreateAsync(UserCreateDto dto);
}