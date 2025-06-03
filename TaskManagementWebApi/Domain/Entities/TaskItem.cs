namespace TaskManagementWebApi.Domain.Entities;

public class TaskItem
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public TaskState State { get; set; }
    
    public int? AssignedUserId { get; set; }
    
    public User? AssignedUser { get; set; }
}