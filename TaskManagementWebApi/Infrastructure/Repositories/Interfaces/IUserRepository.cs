namespace TaskManagementWebApi.Infrastructure.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    
    Task<User[]> GetAllAsync();
//name unique
    Task<User?> GetByNameAsync(string name);
    
    Task<User?> GetByIdAsync(int id);
}