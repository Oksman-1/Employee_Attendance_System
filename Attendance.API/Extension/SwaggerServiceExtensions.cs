using Microsoft.OpenApi.Models;

namespace Attendance.API.Extension;

public static class SwaggerServiceExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Employee_Attendance API",
                Version = "v1",
                Description = "API documentation for Employee Attendance System"
            });
        });

        return services;
    }
}