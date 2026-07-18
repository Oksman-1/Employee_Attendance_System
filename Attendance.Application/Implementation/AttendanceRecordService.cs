using Attendance.Application.Abstractions.Repositories;
using Attendance.Application.Abstractions.Services;
using Attendance.Application.Dto;
using Attendance.Domain.Entities;
using Attendance.Shared.GenericResponse;
using Microsoft.Extensions.Logging;

namespace Attendance.Application.Implementation;

public class AttendanceRecordService(ILogger<AttendanceRecordService> logger, IAttendanceRepository attendanceRepository) : IAttendanceRecordService
{

    private readonly ILogger<AttendanceRecordService> _logger = logger ?? throw new ArgumentException(nameof(ILogger<AttendanceRecordService>));
    private readonly IAttendanceRepository _attendanceRepository = attendanceRepository ?? throw new ArgumentException(nameof(IAttendanceRepository));


    public async Task<GenericResponse<string>> CreateAttendanceRecordAsync(CreateAttendanceRecordDto createAttendanceRecordDto, CancellationToken ct = default)
   {
        _logger.LogInformation($"==============Inside {nameof(CreateAttendanceRecordAsync)}==============");

        _logger.LogInformation("Creating attendance record for EmployeeId {EmployeeId} on {Date}", createAttendanceRecordDto.EmployeeId, createAttendanceRecordDto.AttendanceDate);

        var existingEmployeeRecord = await _attendanceRepository.GetEmployeeAndDateAsync(createAttendanceRecordDto.EmployeeId, createAttendanceRecordDto.AttendanceDate, ct);

        if (existingEmployeeRecord != null)
        {
            _logger.LogWarning("Attendance record already exists for EmployeeId {EmployeeId} on {Date}", createAttendanceRecordDto.EmployeeId, createAttendanceRecordDto.AttendanceDate);

            return GenericResponse<string>.Duplicate
                ($"Attendance record already exists for EmployeeId {createAttendanceRecordDto.EmployeeId} on {createAttendanceRecordDto.AttendanceDate}");
        }
        
        // Map DTO to entity
        var attendanceRecordDto = new AttendanceRecord
        {
            EmployeeId = createAttendanceRecordDto.EmployeeId,
            AttendanceDate = createAttendanceRecordDto.AttendanceDate,
            ClockInAtUtc = createAttendanceRecordDto.ClockInAtUtc,
            ClockOutAtUtc = createAttendanceRecordDto.ClockOutAtUtc,
            HoursWorked = createAttendanceRecordDto.HoursWorked,
            Notes = createAttendanceRecordDto.Notes
        };

        await _attendanceRepository.AddAsync(attendanceRecordDto, ct);
        _logger.LogInformation("Attendance record created successfully for EmployeeId {EmployeeId} on {Date}", createAttendanceRecordDto.EmployeeId, createAttendanceRecordDto.AttendanceDate);

        return GenericResponse<string>.Success("Attendance record created successfully.", null, "201");  
    }

  public async Task<GenericResponse<string>> UpdateAttendanceRecordAsync(UpdateAttendanceRecordDto updateAttendanceRecordDto, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(UpdateAttendanceRecordAsync)}==============");

        _logger.LogInformation("Updating attendance record with Id {Id}", updateAttendanceRecordDto.Id);

        var existingRecord = await _attendanceRepository.GetByIdAsync(updateAttendanceRecordDto.Id,ct);
        if (existingRecord == null)
        {
            _logger.LogWarning("Attendance record with Id {Id} not found", updateAttendanceRecordDto.Id);

            return GenericResponse<string>.NotFound($"Attendance record with Id {updateAttendanceRecordDto.Id} not found.");
        }


        // Update fields
        existingRecord.AttendanceDate = updateAttendanceRecordDto.AttendanceDate;
        existingRecord.ClockInAtUtc = updateAttendanceRecordDto.ClockInAtUtc;
        existingRecord.ClockOutAtUtc = updateAttendanceRecordDto.ClockOutAtUtc;

        if (existingRecord.ClockInAtUtc.HasValue && existingRecord.ClockOutAtUtc.HasValue)
        {
            var totalHours = (existingRecord.ClockOutAtUtc.Value - existingRecord.ClockInAtUtc.Value).TotalHours;
            existingRecord.HoursWorked = Math.Round((decimal)totalHours, 2); // store decimal with 2dp
        }
        else
        {
            existingRecord.HoursWorked = updateAttendanceRecordDto.HoursWorked; // fallback to DTO value if needed
        }
        
        existingRecord.Notes = updateAttendanceRecordDto.Notes;

        await _attendanceRepository.UpdateAsync(existingRecord, ct);

        _logger.LogInformation("Attendance record with Id {Id} updated successfully", updateAttendanceRecordDto.Id);

        return GenericResponse<string>.Success("Attendance record updated successfully.", null, "200"); 

    }

  public async Task<GenericResponse<AttendanceRecordDto>> GetEmployeeAttendanceByDateAsync(int employeeId, DateOnly date, CancellationToken ct = default)
    {
        _logger.LogInformation($"==============Inside {nameof(GetEmployeeAttendanceByDateAsync)}==============");

        _logger.LogInformation("Fetching attendance record for EmployeeId {EmployeeId} on {Date}", employeeId, date);

        var employeeRecord = await _attendanceRepository.GetEmployeeAndDateAsync(employeeId, date, ct);

        if (employeeRecord == null)
        {
            _logger.LogWarning("Attendance record not found for EmployeeId {EmployeeId} on {Date}", employeeId, date);

            return GenericResponse<AttendanceRecordDto>.NotFound($"Attendance record not found for EmployeeId {employeeId} on {date}.");
        }

        var attendanceRecordDto = new AttendanceRecordDto
        (
            Id: employeeRecord.Id,
            EmployeeId: employeeRecord.EmployeeId,
            EmployeeName: employeeRecord.Employee?.FullName ?? string.Empty,
            AttendanceDate: employeeRecord.AttendanceDate,
            ClockInAtUtc: employeeRecord.ClockInAtUtc,
            ClockOutAtUtc: employeeRecord.ClockOutAtUtc,
            HoursWorked:employeeRecord.HoursWorked,
            CalculatedHoursWorked:employeeRecord.CalculatedHoursWorked,
            IsLate:employeeRecord.IsLate,
            Notes:employeeRecord.Notes
        );

        _logger.LogInformation("Attendance record found for EmployeeId {EmployeeId} on {Date}", employeeId, date);

        return GenericResponse<AttendanceRecordDto>.Success("Attendance record retrieved successfully.", attendanceRecordDto, "200");

    }

  public async Task<GenericResponse<IEnumerable<AttendanceRecordDto>>> GetAttendanceRecordsByDateRangeAsync(DateOnly start, DateOnly end, CancellationToken ct = default)
   {
        _logger.LogInformation($"==============Inside {nameof(GetAttendanceRecordsByDateRangeAsync)}==============");

        _logger.LogInformation("Fetching attendance records between {Start} to {End}", start, end);

        var records = await _attendanceRepository.GetByDateRangeAsync(start, end, ct);
        if (records == null || !records.Any())
        {
            _logger.LogWarning("No attendance records found between {Start} to {End}", start, end);

            return GenericResponse<IEnumerable<AttendanceRecordDto>>.NotFound($"No attendance records found between {start} to {end}.");
        }

        var attendanceRecordDtos = records.Select(record => new AttendanceRecordDto
        (
            record.Id,
            record.EmployeeId,
            record.Employee?.FullName ?? string.Empty,
            record.AttendanceDate,
            record.ClockInAtUtc,
            record.ClockOutAtUtc,
            record.HoursWorked,
            record.CalculatedHoursWorked,
            record.IsLate,
            record.Notes
        )).ToList();

        _logger.LogInformation("{Count} attendance records found between {Start} to {End}", attendanceRecordDtos.Count, start, end);

        return GenericResponse<IEnumerable<AttendanceRecordDto>>.Success("Attendance records retrieved successfully.", attendanceRecordDtos, "200");

    }


//    private static AttendanceRecordDto ToDto(AttendanceRecord record) =>
//         new(
//             record.Id,
//             record.EmployeeId,
//             record.Employee?.FullName ?? string.Empty,
//             record.AttendanceDate,
//             record.ClockInAtUtc,
//             record.ClockOutAtUtc,
//             record.HoursWorked,
//             record.CalculatedHoursWorked,
//             record.IsLate,
//             record.Notes
//         );
}