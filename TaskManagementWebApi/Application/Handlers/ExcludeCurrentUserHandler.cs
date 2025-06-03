using TaskManagementWebApi.Domain.Entities;

namespace TaskManagementWebApi.Application.Handlers;

public class ExcludeCurrentUserHandler : AbstractCandidateHandler
{
    protected override IQueryable<User> Process(IQueryable<User> users, TaskItem task, IEnumerable<TaskAssignmentHistory> _)
    {
        if (task.AssignedUserId == null)
            return users;

        return users.Where(u => u.Id != task.AssignedUserId);
    }
}