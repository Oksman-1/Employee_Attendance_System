using Attendance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Attendance.Infrastructure.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        //Table Mapping
        builder.ToTable("Employees", "Attendance");
        
        //Primary Key
        builder.HasKey(e => e.Id);
        
        //Properties
        builder.Property(e => e.Id)
            .HasColumnName("ID");
        
        builder.Property(e => e.EmployeeCode)
            .HasColumnName("EMPLOYEE_CODE")
            .HasMaxLength(50)
            .IsRequired();
        
        builder.HasIndex(e => e.EmployeeCode).IsUnique();
        
        builder.Property(e => e.FullName)
            .HasColumnName("FULL_NAME")
            .HasMaxLength(120)
            .IsRequired();
        
        builder.Property(e => e.Email)
            .HasColumnName("EMAIL")
            .HasMaxLength(256)
            .IsRequired();
        
        builder.HasIndex(e => e.Email).IsUnique();

        builder.Property(e => e.Department)
            .HasColumnName("DEPARTMENT")
            .HasMaxLength(100);
        
        builder.Property(e => e.JobTitle)
            .HasColumnName("JOB_TITLE")
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(e => e.QrCode)
            .HasColumnName("QR_CODE")
            .HasMaxLength(128)
            .IsRequired();

        builder.HasIndex(e => e.QrCode).IsUnique();
        
        builder.Property(e => e.HireDate)
            .HasColumnName("HIRE_DATE")
            .IsRequired();
        
        builder.Property(e => e.CreatedAtUtc)
            .HasColumnName("CREATED_AT")
            .HasConversion(v => v, v => DateTime.SpecifyKind(v.DateTime, DateTimeKind.Utc))
            .IsRequired();
        
        // SQL Server rowversion/timestamp
        builder.Property(e => e.RowVersion)
            .HasColumnName("ROW_VERSION")
            .IsRowVersion();
        
        builder.Property(e => e.IsActive)
            .HasColumnName("IS_ACTIVE")
            .HasDefaultValue(true);
        
        //Relationships
        builder.HasMany(e => e.AttendanceRecords)
            .WithOne(r => r.Employee)
            .HasForeignKey(r => r.EmployeeId);
        
    }
}