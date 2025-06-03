using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TaskManagementWebApi.Application.Handlers;
using TaskManagementWebApi.Application.Observers;
using TaskManagementWebApi.Application.States;
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
                var chainBuilder = scope.ServiceProvider.GetRequiredService<CandidateHandlerChainBuilder>();
                var publisher = scope.ServiceProvider.GetRequiredService<TaskEventPublisher>();

                await ReassignTasksAsync(db, chainBuilder, publisher, stoppingToken);
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
    
    private async Task ReassignTasksAsync(
        TaskManagementDbContext db,
        CandidateHandlerChainBuilder chainBuilder,
        TaskEventPublisher publisher,
        CancellationToken token)
    {
        var tasks = await db.Tasks
            .Where(t => t.State == TaskState.InProgress)
            .ToArrayAsync(token);

        var handler = chainBuilder.Build();
        var rnd = new Random();

        foreach (var task in tasks)
        {
            var history = await db.TaskAssignments
                .Where(h => h.TaskItemId == task.Id)
                .OrderByDescending(h => h.AssignedAt)
                .ToArrayAsync(token);

            var previousState = task.State;
            var previousUserId = task.AssignedUserId;
            
            var candidatesQuery = db.Users.AsQueryable();
            candidatesQuery = handler.Handle(candidatesQuery, task, history);

            var candidates = await candidatesQuery.ToArrayAsync(token);

            if (candidates.Length == 0)
            {
                var allUserIds = history
                    .Where(h => h.AssignedUserId.HasValue)
                    .Select(h => h.AssignedUserId!.Value)
                    .Distinct()
                    .ToHashSet();

                var allUserIdsInDb = await db.Users.Select(u => u.Id).ToListAsync(token);

                if (allUserIdsInDb.All(uid => allUserIds.Contains(uid)))
                {
                    task.State = TaskState.Completed;
                    task.AssignedUserId = null;
                }

                await publisher.PublishAsync(task, new TaskChangeContext
                {
                    PreviousState = previousState,
                    PreviousUserId = previousUserId
                }, token);

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

            await publisher.PublishAsync(task, new TaskChangeContext
            {
                PreviousState = previousState,
                PreviousUserId = previousUserId
            }, token);
        }

        await db.SaveChangesAsync(token);
    }
}

