using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TaskManagementWebApi.Domain.Entities;
using TaskManagementWebApi.Infrastructure.Persistence;
using TaskManagementWebApi.Infrastructure.Repositories.Interfaces;

namespace TaskManagementWebApi.Application.Services;

public class TaskReassignmentService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TaskReassignmentService> _logger;

    public TaskReassignmentService(IServiceScopeFactory scopeFactory, ILogger<TaskReassignmentService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<TaskManagementDbContext>();
                await ReassignTasksAsync(db, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reassignment failed.");
            }

            var delay = TimeSpan.FromMinutes(2) - sw.Elapsed;
            if (delay > TimeSpan.Zero)
                await Task.Delay(delay, stoppingToken);
        }
    }

    private async Task ReassignTasksAsync(TaskManagementDbContext db, CancellationToken token)
    {
        var tasks = await db.Tasks
            .Where(t => t.State == TaskState.InProgress)
            .ToArrayAsync(token);

        var users = await db.Users.ToArrayAsync(token);
        var rnd = new Random();

        foreach (var task in tasks)
        {
            var history = await db.TaskAssignments
                .Where(h => h.TaskItemId == task.Id)
                .OrderByDescending(h => h.AssignedAt)
                .ToArrayAsync(token);

            var prevUserId = history.Skip(1).FirstOrDefault()?.AssignedUserId;
            var currentUserId = task.AssignedUserId;

            var candidates = users
                .Where(u => u.Id != currentUserId && u.Id != prevUserId)
                .ToArray();

            if (candidates.Length == 0)
            {
                var allUserIds = history
                    .Where(h => h.AssignedUserId.HasValue)
                    .Select(h => h.AssignedUserId!.Value)
                    .Distinct()
                    .ToHashSet();

                if (users.All(u => allUserIds.Contains(u.Id)))
                {
                    task.State = TaskState.Completed;
                    task.AssignedUserId = null;
                }

                continue;
            }

            var newUser = candidates[rnd.Next(candidates.Length)];

            task.AssignedUserId = newUser.Id;
            task.State = TaskState.InProgress;

            db.TaskAssignments.Add(new TaskAssignmentHistory
            {
                TaskItemId = task.Id,
                AssignedUserId = newUser.Id,
                AssignedAt = DateTime.UtcNow
            });
        }

        await db.SaveChangesAsync(token);
    }
}

