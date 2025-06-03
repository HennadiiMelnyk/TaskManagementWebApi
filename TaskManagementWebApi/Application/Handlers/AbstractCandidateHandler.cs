using TaskManagementWebApi.Application.Handlers.Interfaces;
using TaskManagementWebApi.Domain.Entities;

namespace TaskManagementWebApi.Application.Handlers;

public abstract class AbstractCandidateHandler : ICandidateSelectionHandler
{
    private ICandidateSelectionHandler? _next;

    public ICandidateSelectionHandler SetNext(ICandidateSelectionHandler next)
    {
        _next = next;
        return next;
    }

    public virtual IQueryable<User> Handle(IQueryable<User> candidates, TaskItem task, IEnumerable<TaskAssignmentHistory> history)
    {
        var filtered = Process(candidates, task, history);
        return _next?.Handle(filtered, task, history) ?? filtered;
    }

    protected abstract IQueryable<User> Process(IQueryable<User> candidates, TaskItem task, IEnumerable<TaskAssignmentHistory> history);
}
