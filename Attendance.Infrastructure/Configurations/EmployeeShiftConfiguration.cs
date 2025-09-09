using Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attendance.Infrastructure.Configurations;

public class EmployeeShiftConfiguration : IEntityTypeConfiguration<EmployeeShift>
{
    public void Configure(EntityTypeBuilder<EmployeeShift> builder)
    {
        builder.ToTable("EmployeeShifts");
        
        //Primary Key
        builder.HasKey(es => es.Id);
        
        //Id auto-generated
        builder.Property(es => es.Id)
            .ValueGeneratedOnAdd();

        builder.Property(es => es.EmployeeId)
            .HasColumnName("EMPLOYEE_ID")
            .IsRequired();
        
        builder.Property(es => es.ShiftId)
            .HasColumnName("SHIFT_ID")
            .IsRequired();
        
        builder.Property(es => es.AssignedDate)
            .HasColumnName("ASSIGNED_DATE")
            .HasColumnType("date")//stores only date portion
            .IsRequired();
        
        //Relationships
        builder.HasOne(es => es.Employee)
            .WithMany(e => e.EmployeeShifts)
            .HasForeignKey(es => es.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade); // If Employee is deleted, their shifts are also deleted
        
        builder.HasOne(es => es.Shift)
            .WithMany(s => s.EmployeeShifts)
            .HasForeignKey(es => es.ShiftId)    
            .OnDelete(DeleteBehavior.Cascade);
        
        // Indexes
        // Optional: Unique constraint to prevent duplicate shift assignment for the same employee on the same date
        builder.HasIndex(es => new { es.EmployeeId, es.AssignedDate})
            .IsUnique()
            .HasDatabaseName("IX_EmployeeShift_EmployeeId_AssignedDate");
    }
}