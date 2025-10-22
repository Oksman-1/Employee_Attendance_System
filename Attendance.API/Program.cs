using Attendance.API.Extension;
using Attendance.Domain.Common;
using Attendance.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

//Configure Serilog 
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log--.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

// Replaces default logging
builder.Host.UseSerilog();

// Bind and validate using DataAnnotations
builder.Services.AddOptions<EmailSettings>()
    .Bind(configuration.GetSection("Email"))
    .ValidateDataAnnotations()
    .Validate(settings => !string.IsNullOrWhiteSpace(settings.SmtpServer), "SmtpServer required")
    .ValidateOnStart(); 

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<ValidationFilterAttribute>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();