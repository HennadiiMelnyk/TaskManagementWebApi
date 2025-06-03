using TaskManagementWebApi.Domain.Entities;

namespace TaskManagementWebApi.Application.Handlers.Interfaces;

public interface ICandidateSelectionHandler
{
    ICandidateSelectionHandler SetNext(ICandidateSelectionHandler next);
    User[] Handle(User[] candidates, TaskItem task, IEnumerable<TaskAssignmentHistory> history);
}