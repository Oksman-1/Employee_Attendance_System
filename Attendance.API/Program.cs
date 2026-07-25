using System.Text.Json;
using Attendance.API.Extension;
using Attendance.Infrastructure;
using Attendance.Shared.Common;
using Attendance.Shared.SerilogEnricher;
using Serilog;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.EnvironmentName == "Dev")
{
    builder.Configuration.AddUserSecrets<Program>();
}

var configuration = builder.Configuration;

//Configure Serilog 
LoggingConfiguration.Configure();

// Log.Logger = new LoggerConfiguration()
//     .WriteTo.Console()
//     .WriteTo.File("Logs/log--.txt", rollingInterval: RollingInterval.Day)
//     .Enrich.FromLogContext()
//     .MinimumLevel.Information()
//     .CreateLogger();

// Replaces default logging
builder.Host.UseSerilog();

// Bind and validate using DataAnnotations
builder.Services.AddOptions<EmailSettings>()
    .Bind(configuration.GetSection("Email"))
    .ValidateDataAnnotations()
    .Validate(settings => !string.IsNullOrWhiteSpace(settings.SmtpServer), "SmtpServer required")
    .ValidateOnStart(); 

// Add services to the container.
//builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddFluentValidationAutoValidation();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddScoped<ValidationFilterAttribute>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);

// Register Swagger generator
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Swagger
builder.Services.AddSwaggerDocumentation();


var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
//     //app.UseSwagger();
//     //app.UseSwaggerUI();
// }

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Dev" ||
    Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "uat")
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseCors("CorsPolicy");

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var identityService = scope.ServiceProvider.GetRequiredService<Attendance.Application.Abstractions.Services.IIdentityService>();
    await identityService.SeedRolesAsync();
    await identityService.SeedDefaultAdminAsync();
    
    var employeeMigrationRepo = scope.ServiceProvider.GetRequiredService<Attendance.Application.Abstractions.Repositories.IEmployeeMigrationRepository>();
    var defaultPinHash = HashHelper.ComputeSha256Hash("1234");
    await employeeMigrationRepo.UpdateEmptyPinHashesAsync(defaultPinHash);
}

app.Run();