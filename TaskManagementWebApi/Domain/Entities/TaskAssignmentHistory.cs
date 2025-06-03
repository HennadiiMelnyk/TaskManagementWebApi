namespace TaskManagementWebApi.Domain.Entities;

public class TaskAssignmentHistory
{
    public int Id { get; set; }
    public int TaskItemId { get; set; }
    public int? AssignedUserId { get; set; }
    public DateTime AssignedAt { get; set; }

    public TaskItem Task { get; set; }
}