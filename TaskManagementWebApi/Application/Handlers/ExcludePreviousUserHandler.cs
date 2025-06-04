using TaskManagementWebApi.Domain.Entities;

namespace TaskManagementWebApi.Application.Handlers;

public class ExcludePreviousUserHandler : AbstractCandidateHandler
{
    protected override IQueryable<User> Process(IQueryable<User> users, TaskItem task, IEnumerable<TaskAssignmentHistory> history)
    {
        var prevUserId = history.Skip(1).FirstOrDefault()?.AssignedUserId;
        return prevUserId == null ? users : users.Where(u => u.Id != prevUserId);
    }
}