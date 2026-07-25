using System.Threading.Tasks;
using Attendance.Application.Abstractions.Repositories;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Attendance.Infrastructure.Repositories;

public class EmployeeMigrationRepository : IEmployeeMigrationRepository
{
    private readonly string _connectionString;
    private readonly ILogger<EmployeeMigrationRepository> _logger;

    public EmployeeMigrationRepository(IConfiguration configuration, ILogger<EmployeeMigrationRepository> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new System.InvalidOperationException("DefaultConnection not found in configuration.");
        _logger = logger;
    }

    public async Task UpdateEmptyPinHashesAsync(string defaultPinHash)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = @"
            UPDATE Attendance.Employees
            SET PinHash = @DefaultPinHash
            WHERE PinHash IS NULL OR PinHash = '';";

        int rowsAffected = await connection.ExecuteAsync(query, new { DefaultPinHash = defaultPinHash });

        if (rowsAffected > 0)
        {
            _logger.LogInformation("Successfully updated {RowsAffected} existing employees with the default PIN hash.", rowsAffected);
        }
        else
        {
            _logger.LogInformation("No existing employees required a PIN hash update.");
        }
    }
}
