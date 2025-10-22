using Attendance.Application.Abstractions;
using Attendance.Application.Abstractions.Repositories;
using Attendance.Application.Abstractions.Services;
using Attendance.Application.DtoValidation;
using Attendance.Application.Implementation;
using Attendance.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Attendance.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //Add Repositories
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IAttendanceRepository, AttendanceRepository>();
        
        //Add Services
         services.AddScoped<IEmployeeService, EmployeeService>();
         services.AddScoped<IEmailService, EmailService>();
        // services.AddScoped<IReportingService, ReportingService>();
        
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