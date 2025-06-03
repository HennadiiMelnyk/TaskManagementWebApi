using TaskManagementWebApi.Domain.Entities;

namespace TaskManagementWebApi.Application.Handlers;

public class ExcludeCurrentUserHandler : AbstractCandidateHandler
{
    protected override User[] Process(User[] candidates, TaskItem task, IEnumerable<TaskAssignmentHistory> _)
    {
        return candidates.Where(u => u.Id != task.AssignedUserId).ToArray();
    }
}