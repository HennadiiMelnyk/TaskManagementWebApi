using TaskManagementWebApi.Application.DTO;
using TaskManagementWebApi.Application.Services.Interfaces;
using TaskManagementWebApi.Infrastructure.Repositories;
using TaskManagementWebApi.Infrastructure.Repositories.Interfaces;

namespace TaskManagementWebApi.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<UserDto> CreateAsync(UserCreateDto dto)
    {
        var existing = await _repo.GetByNameAsync(dto.Name);
        if (existing != null)
            throw new InvalidOperationException("User already exists");

        var user = new User { Name = dto.Name };
        var result = await _repo.AddAsync(user);

        return new UserDto { Id = result.Id, Name = result.Name };
    }
    
    public Task<User> AddAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto[]> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}