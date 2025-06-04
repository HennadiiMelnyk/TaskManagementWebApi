using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagementWebApi.Domain.Entities;

namespace TaskManagementWebApi.Infrastructure.Configurations;

public class TaskAssignmentHistoryConfigurations : IEntityTypeConfiguration<TaskAssignmentHistory>
{
    public void Configure(EntityTypeBuilder<TaskAssignmentHistory> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.AssignedAt)
            .IsRequired();

        builder.HasOne(h => h.Task)
            .WithMany()
            .HasForeignKey(h => h.TaskItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}