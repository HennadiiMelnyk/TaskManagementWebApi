using Microsoft.EntityFrameworkCore;
using TaskManagementWebApi.Infrastructure.Persistence;
using TaskManagementWebApi.Infrastructure.Repositories.Interfaces;

namespace TaskManagementWebApi.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TaskManagementDbContext _context;
    
    public UserRepository(TaskManagementDbContext context) => _context = context;

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public Task<User[]> GetAllAsync() => _context.Users.ToArrayAsync();
    
    public Task<User?> GetByNameAsync(string name) =>
        _context.Users.FirstOrDefaultAsync(u => u.Name == name);

    public Task<User?> GetByIdAsync(int id)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}