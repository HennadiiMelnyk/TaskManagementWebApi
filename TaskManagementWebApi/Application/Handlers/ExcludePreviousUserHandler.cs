using TaskManagementWebApi.Domain.Entities;

namespace TaskManagementWebApi.Application.Handlers;

public class ExcludePreviousUserHandler : AbstractCandidateHandler
{
    protected override User[] Process(User[] candidates, TaskItem task, IEnumerable<TaskAssignmentHistory> history)
    {
        var previousUserId = history.Skip(1).FirstOrDefault()?.AssignedUserId;
        return previousUserId == null ? candidates : candidates.Where(u => u.Id != previousUserId).ToArray();
    }
}