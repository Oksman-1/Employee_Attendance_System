using Attendance.Application.Abstractions;
using Attendance.Application.Abstractions.Repositories;
using Attendance.Application.Abstractions.Services;
using Attendance.Application.DtoValidation;
using Attendance.Application.Implementation;
using Attendance.Infrastructure.Persistence;
using Attendance.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;


namespace Attendance.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //Add DbContext
         services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        
        //Add Repositories
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        services.AddScoped<IEmployeeShiftRepository, EmployeeShiftRepository>();
        services.AddScoped<ILeaveRecordRepository, LeaveRecordRepository>();
        services.AddScoped<IShiftRepository, ShiftRepository>();
        
        
        
        //services.AddScoped<>();
        
        //Add Services
         services.AddScoped<IEmployeeService, EmployeeService>();
         services.AddScoped<IEmailService, EmailService>();
         services.AddScoped<IAttendanceRecordService, AttendanceRecordService>();
         services.AddScoped<IEmployeeShiftService, EmployeeShiftService>();
         services.AddScoped<ILeaveRecordService, LeaveRecordService>();
         services.AddScoped<IShiftService, ShiftService>();
         services.AddScoped<IReportingService, ReportingService>();
        
        // Register FluentValidation validators
        services.AddValidatorsFromAssemblyContaining<CreateEmployeeDtoValidator>();

        
        //Add CORS (allow any origin)
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });
        
      
        
        return services;
    }
}