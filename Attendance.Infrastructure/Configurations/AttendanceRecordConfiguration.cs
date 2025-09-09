using Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attendance.Infrastructure.Configurations;

public class AttendanceRecordConfiguration : IEntityTypeConfiguration<AttendanceRecord>
{
    public void Configure(EntityTypeBuilder<AttendanceRecord> builder)
    {
        builder.ToTable("AttendanceRecords");
        
        //Primary key
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();
        
        //Properties
        // AttendanceDate stored as SQL 'date'
        builder.Property(a => a.AttendanceDate)
            .HasColumnName("ATTENDANCE_DATE")
            .HasColumnType("date")
            .IsRequired();
        
        builder.Property(a => a.HoursWorked)
            .HasColumnName("HOURS_WORKED")
            .HasColumnType("DECIMAL(5,2)")
            .HasDefaultValue(0)
            .IsRequired();
        
        // Clock In/Out in UTC
        builder.Property(a => a.ClockInAtUtc)
            .HasColumnName("CLOCK_IN_UTC")
            .HasColumnType("datetimeoffset");

        builder.Property(a => a.ClockOutAtUtc)
            .HasColumnName("CLOCK_OUT_UTC")
            .HasColumnType("datetimeoffset");

        builder.Property(a => a.Notes)
            .HasColumnName("NOTES")
            .HasMaxLength(400);
        
        builder.Property(a => a.RowVersion)
            .HasColumnName("ROW_VERSION")
            .IsRowVersion()
            .IsConcurrencyToken();
        
        // Ignore computed properties (HoursWorked & IsLate)
        builder.Ignore(a => a.HoursWorked);
        builder.Ignore(a => a.IsLate);
        
        // Prevent duplicate records for same employee/day. It enforces one record per employee per day at the database level.
        builder.HasIndex(a => new { a.EmployeeId ,a.AttendanceDate })
            .IsUnique();
        
        // Employee Relationship
        builder.HasOne(a => a.Employee)
            .WithMany(a => a.AttendanceRecords)
            .HasForeignKey(a => a.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Check constraint: ensures each employee can have only one record per day
        builder.ToTable(t =>
        {
            t.HasCheckConstraint(
                "CK_Attendance_ClockTimes_Valid",
                "[CLOCK_OUT_UTC] IS NULL OR [CLOCK_OUT_UTC] >= [CLOCK_IN_UTC]"
            );
        });
    }
}