using Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attendance.Infrastructure.Configurations;

public class LeaveRecordConfiguration : IEntityTypeConfiguration<LeaveRecord>
{
    public void Configure(EntityTypeBuilder<LeaveRecord> builder)
    {
        builder.ToTable("LeaveRecords", table =>
        {
            table.HasCheckConstraint("CK_LeaveRecord_DateRange", "[EndDate] >= [StartDate]");
        });


        // Primary Key
        builder.HasKey(lr => lr.Id);
        
        builder.Property(lr => lr.Id)
            .ValueGeneratedOnAdd();

        builder.Property(lr => lr.EmployeeId)
            .HasColumnName("EMPLOYEE_ID")
            .IsRequired();
        
        builder.Property(lr => lr.StartDate)
            .HasColumnName("START_DATE")
            .HasColumnType("DATE")
            .IsRequired();

        builder.Property(lr => lr.EndDate)
            .HasColumnName("END_DATE")
            .HasColumnType("DATE")
            .IsRequired();

        builder.Property(lr => lr.Reason)
            .HasColumnName("REASON")
            .HasMaxLength(500)
            .IsRequired();

        // Approval flag
        builder.Property(lr => lr.Approved)
            .HasColumnName("APPROVED")
            .HasDefaultValue(false)
            .IsRequired();
        
        builder.HasOne(lr => lr.Employee)
            .WithMany(e => e.LeaveRecords)
            .HasForeignKey(lr => lr.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}