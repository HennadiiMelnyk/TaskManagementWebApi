namespace TaskManagementWebApi.Application.DTO;

public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string State { get; set; }
    public int? AssignedUserId { get; set; }
}