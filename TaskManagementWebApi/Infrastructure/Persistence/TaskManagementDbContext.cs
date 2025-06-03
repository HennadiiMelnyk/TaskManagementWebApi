using Microsoft.EntityFrameworkCore;
using TaskManagementWebApi.Domain.Entities;

namespace TaskManagementWebApi.Infrastructure.Persistence;

public class TaskManagementDbContext : DbContext
{
    public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    
    public virtual DbSet<TaskItem> Tasks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TaskManagementDbContext).Assembly);
    }
}