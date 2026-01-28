using Attendance.Application.Abstractions.Repositories;
using Attendance.Application.Abstractions.Services;
using Attendance.Application.Dto;
using Attendance.Domain.Entities;
using Attendance.Shared.GenericResponse;
using Microsoft.Extensions.Logging;
using QRCoder;


namespace Attendance.Application.Implementation;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<EmployeeService> _logger;
    
    
    public EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService> logger)
    {
        _employeeRepository = employeeRepository;
        _logger = logger;
    }
    
    public async Task<GenericResponse<EmployeeDto>> GetEmployeeByIdAsync(int id, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetEmployeeByIdAsync)}==============");

        _logger.LogInformation("Fetching employee with ID: {EmployeeId}", id);
        
        var employee = await _employeeRepository.GetByIdAsync(id, ct);
        
        if (employee == null)
        {
            _logger.LogInformation("Employee with ID: {EmployeeId} not found", id);
            
            return GenericResponse<EmployeeDto>.NotFound("Employee not found");
        }

        var employeeDto = new EmployeeDto(
            employee.Id,
            employee.EmployeeCode,
            employee.FullName,
            employee.Email,
            employee.Department,
            employee.JobTitle,
            employee.HireDate,
            employee.IsActive,
            employee.CreatedAtUtc
        );

        _logger.LogInformation("Successfully retrieved employee {@employee}", employee);
        
        return GenericResponse<EmployeeDto>.Success("Success", employeeDto, "200");
    }

    public async Task<GenericResponse<EmployeeDto>> GetEmployeeByQrCodeAsync(string qrCode, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetEmployeeByQrCodeAsync)}==============");

        _logger.LogInformation("Fetching employee by QR Code {QrCode}", qrCode);
        
        var employee = await _employeeRepository.GetByCodeAsync(qrCode, ct);
        if (employee == null)
        {
            _logger.LogInformation("Employee with QR Code: {QrCode} not found", qrCode);
            
            return GenericResponse<EmployeeDto>.NotFound("Employee not found");
        }

        var employeeDto = new EmployeeDto(
            employee.Id,
            employee.EmployeeCode,
            employee.FullName,
            employee.Email,
            employee.Department,
            employee.JobTitle,
            employee.HireDate,
            employee.IsActive,
            employee.CreatedAtUtc
        );

        _logger.LogInformation("Successfully retrieved employee {@employee}", employee);
        
        return GenericResponse<EmployeeDto>.Success("Success", employeeDto, "200");
    }

    public async Task<GenericResponse<List<EmployeeDto>>> GetAllEmployeesAsync(CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetAllEmployeesAsync)}==============");
        
        _logger.LogInformation("Fetching all employees");
        
        var employees = await _employeeRepository.GetAllAsync(ct);
        
        if (employees == null || employees.Count == 0)
        {
            _logger.LogInformation("No employees found");
            
            return GenericResponse<List<EmployeeDto>>.NotFound("No employees found");
        }

        var employeeDtos = employees.Select(e => new EmployeeDto(
            e.Id,
            e.EmployeeCode,
            e.FullName,
            e.Email,
            e.Department,
            e.JobTitle,
            e.HireDate,
            e.IsActive,
            e.CreatedAtUtc
        )).ToList();
        
        _logger.LogInformation("Successfully retrieved {Count} employees", employees.Count);
        
        return GenericResponse<List<EmployeeDto>>.Success("Success", employeeDtos, "200");
    }

    public async Task<GenericResponse<string>> CreateEmployeeAsync(CreateEmployeeDto createEmployeeDto, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(CreateEmployeeAsync)}==============");
        
        _logger.LogInformation("Creating new employee with employeeCode: {employeeCode}", createEmployeeDto.EmployeeCode);
        
        // Check for employee with same employeeCode
        var existingEmployee = await _employeeRepository.GetByCodeAsync(createEmployeeDto.EmployeeCode, ct);
        
        if (existingEmployee is not null)
        {
            _logger.LogWarning("Duplicate employee with employeeCode {employeeCode} detected", createEmployeeDto.EmployeeCode);
            
            return GenericResponse<string>.Duplicate($"An employee with employee code: {createEmployeeDto.EmployeeCode} already exists");
        }
        
        // Generate a new QR code value (this should be a unique, URL-safe string)
        var qrCodeValue = GenerateQrCode(createEmployeeDto.EmployeeCode);
        
        var newEmployee = new Employee
        {
            EmployeeCode = createEmployeeDto.EmployeeCode,
            FullName = createEmployeeDto.FullName,
            Email = createEmployeeDto.Email,
            Department = createEmployeeDto.Department,
            JobTitle = createEmployeeDto.JobTitle,
            HireDate = createEmployeeDto.HireDate,
            IsActive = true,
        };
        
        newEmployee.AssignNewQrCode(qrCodeValue);

        await _employeeRepository.AddAsync(newEmployee, ct);
        
        _logger.LogInformation("Successfully created employee {@employee}", newEmployee);
        
        return GenericResponse<string>.Success("Employee created successfully", null, "200");
    }

    public async Task<GenericResponse<string>> UpdateEmployeeAsync(UpdateEmployeeDto updateEmployeeDto, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(UpdateEmployeeAsync)}==============");
        
        _logger.LogInformation("Updating employee with ID: {EmployeeId}", updateEmployeeDto.EmployeeId);
        
        var existingEmployee = await _employeeRepository.GetByIdAsync(updateEmployeeDto.EmployeeId, ct);
        
        if (existingEmployee == null)
        {
            _logger.LogWarning("Employee with ID: {EmployeeId} not found", updateEmployeeDto.EmployeeId);
            
            return GenericResponse<string>.NotFound("Employee not found");
        }
        
        existingEmployee.FullName = updateEmployeeDto.FullName;
        existingEmployee.Email = updateEmployeeDto.Email;
        existingEmployee.Department = updateEmployeeDto.Department;
        existingEmployee.JobTitle = updateEmployeeDto.JobTitle;
        existingEmployee.IsActive = updateEmployeeDto.IsActive;
        
        await _employeeRepository.UpdateAsync(existingEmployee, ct);
        
        _logger.LogInformation("Successfully updated employee {@employee}", existingEmployee);
        
        return GenericResponse<string>.Success("Employee updated successfully", null, "200");
    }

    public async Task<GenericResponse<string>> DeleteEmployeeAsync(int id, CancellationToken ct = default)
    {
        
        _logger.LogInformation($"==============Inside {nameof(DeleteEmployeeAsync)}==============");
        
        _logger.LogInformation("Deleting employee with ID: {EmployeeId}", id);
        
        var existingEmployee = await _employeeRepository.GetByIdAsync(id, ct);
        
        if (existingEmployee == null)
        {
            _logger.LogWarning("Employee with ID: {EmployeeId} not found", id);
            
            return GenericResponse<string>.NotFound("Employee not found");
        }
        
        await _employeeRepository.DeleteAsync(existingEmployee, ct);
        
        _logger.LogInformation("Successfully deleted employee with ID: {EmployeeId}", id);
        
        return GenericResponse<string>.Success("Employee deleted successfully", null, "200");
    }
    
    private string GenerateQrCode(string employeeCode)
    {
        //Create unique QR value
        var qrCodeValue = $"EMP-{Guid.NewGuid():N}";
        
        //Create QR image file path
        var qrFileName = $"{employeeCode}_{DateTime.UtcNow:yyyyMMddHHmmss}_qr.png";
        var qrFilePath = Path.Combine("wwwroot","QRCodes", qrFileName);
        Directory.CreateDirectory(Path.GetDirectoryName(qrFilePath));
        
        //WINDOWS - Requires System.Drawing.Common NuGet package
        //Generate QR image
        // using var qrGenerator = new QRCodeGenerator();
        // using var qrData = qrGenerator.CreateQrCode(qrCodeValue, QRCodeGenerator.ECCLevel.Q);
        // using var qrCode = new QRCode(qrData);
        // using var qrImage = qrCode.GetGraphic(20);
        //
        // //Save QR image to file
        // qrImage.Save(qrFilePath, System.Drawing.Imaging.ImageFormat.Png);
        //
        // _logger.LogInformation("Generated QR code for employee {EmployeeCode} at {Path}", employeeCode, qrFilePath);
        //
        // return qrCodeValue;
        
        //CROSS-PLATFORM - Requires QRCoder NuGet package
        using var qrGenerator = new QRCodeGenerator();
        using var data = qrGenerator.CreateQrCode(qrCodeValue, QRCodeGenerator.ECCLevel.Q);

        var pngQr = new PngByteQRCode(data);
        byte[] pngBytes = pngQr.GetGraphic(pixelsPerModule: 20);

        File.WriteAllBytes(qrFilePath, pngBytes);
        
        _logger.LogInformation("Generated QR code for employee {EmployeeCode} at {Path}", employeeCode, qrFilePath);

        return qrCodeValue;
    }
}