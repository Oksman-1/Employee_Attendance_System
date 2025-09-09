using Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attendance.Infrastructure.Configurations;

public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
{
    public void Configure(EntityTypeBuilder<Shift> builder)
    {
        builder.ToTable("Shifts");
        
        //Primary Key
        builder.HasKey(s => s.Id);
        
        // Id auto-generated
        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();  
        
        //Properties
        builder.Property(s => s.Name)
            .HasColumnName("NAME")
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(s => s.StartTime)
            .HasColumnName("START_TIME")
            .IsRequired();
            
        builder.Property(s => s.EndTime)
            .HasColumnName("END_TIME")
            .IsRequired();
            
        builder.Property(s => s.GracePeriodMinutes)
            .HasColumnName("GRACE_PERIOD_MINUTES")
            .HasDefaultValue(10)
            .IsRequired();
        
        // Index on Name for quick lookup
        builder.HasIndex(s => s.Name)
            .IsUnique();
        
        builder.HasMany(s => s.EmployeeShifts)
            .WithOne(es => es.Shift)
            .HasForeignKey(es => es.ShiftId)
            .OnDelete(DeleteBehavior.Cascade);
        
        //Constraint to ensure EndTime > StartTime
        builder.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_Shift_StartEnd",
                "[END_TIME] > [START_TIME]"
            );

        });
    }
}