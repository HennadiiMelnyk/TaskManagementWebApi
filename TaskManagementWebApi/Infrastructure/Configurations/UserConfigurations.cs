using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskManagementWebApi.Infrastructure.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Name);
        
        builder.HasMany(e => e.Tasks)
            .WithOne(e => e.AssignedUser)
            .HasForeignKey(e => e.AssignedUserId);

    }
}