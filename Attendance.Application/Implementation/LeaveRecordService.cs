using Attendance.Application.Abstractions.Repositories;
using Attendance.Application.Abstractions.Services;
using Attendance.Application.Dto;
using Attendance.Domain.Common;
using Attendance.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Attendance.Application.Implementation;

public class LeaveRecordService : ILeaveRecordService
{
      private readonly ILeaveRecordRepository _leaveRecordRepository;
      private readonly ILogger<LeaveRecordService> _logger;

      public LeaveRecordService(ILeaveRecordRepository leaveRecordRepository, ILogger<LeaveRecordService> logger)
      {
        _leaveRecordRepository = leaveRecordRepository;
        _logger = logger;
      }

        public async Task<GenericResponse<string>> CreateLeaveRecordAsync(CreateLeaveRecordDto dto, CancellationToken ct = default)
      {
          _logger.LogInformation("==============Inside {Method}==============", nameof(CreateLeaveRecordAsync));

          _logger.LogInformation("About to create leave record for employe with employee Id: {employeeId}", dto.EmployeeId);

          if(await _leaveRecordRepository.HasOverlappingLeaveAsync(dto.EmployeeId, dto.StartDate, dto.EndDate,ct))
          {
              _logger.LogWarning("Overlapping leave detected for EmployeeId: {EmployeeId}", dto.EmployeeId);

              return GenericResponse<string>.Duplicate("Employee already has leave in the given period.");
          }

          var leaveRecord = new LeaveRecord
          {
              EmployeeId = dto.EmployeeId,
              StartDate = dto.StartDate,
              EndDate = dto.EndDate,
              Reason = dto.Reason,
              Approved = false
          };

          await _leaveRecordRepository.CreateLeaveRecordAsync(leaveRecord, ct);

          _logger.LogInformation("LeaveRecord created successfully for EmployeeId: {EmployeeId}", dto.EmployeeId);

          return GenericResponse<string>.Success("Leave record created successfully", null, "200");
      }

      public async Task<GenericResponse<string>> UpdateLeaveRecordAsync(UpdateLeaveRecordDto dto, CancellationToken ct = default)
      {
          _logger.LogInformation("==============Inside {Method}==============", nameof(UpdateLeaveRecordAsync));

          _logger.LogInformation("About to update leave record with Id: {Id}", dto.Id);

          var existingLeaveRecord = await _leaveRecordRepository.GetLeaveRecordByIdAsync(dto.Id, ct);
          if(existingLeaveRecord is null)
          {
            _logger.LogWarning("LeaveRecord with Id {Id} not found", dto.Id);

            return GenericResponse<string>.NotFound("Leave record not found");
          }

          existingLeaveRecord.StartDate = dto.StartDate;
          existingLeaveRecord.EndDate = dto.EndDate;
          existingLeaveRecord.Reason = dto.Reason;

          await _leaveRecordRepository.UpdateLeaveRecordAsync(existingLeaveRecord,ct);

          _logger.LogInformation("LeaveRecord with Id {Id} updated successfully", dto.Id);

          return GenericResponse<string>.Success("Leave record updated successfully", null, "200");
      }

      public async Task<GenericResponse<string>> DeleteLeaveRecordAsync(int leaveRecordId, CancellationToken ct = default)
      {
          _logger.LogInformation("==============Inside {Method}==============", nameof(DeleteLeaveRecordAsync));
          
          _logger.LogInformation("About to delete leave record with id: {leaveRecordId}", leaveRecordId);
          
          var existingLeaveRecord = await _leaveRecordRepository.GetLeaveRecordByIdAsync(leaveRecordId, ct);
          if (existingLeaveRecord is null)
          {
            _logger.LogWarning("LeaveRecord with Id {Id} not found", leaveRecordId);
            
            return GenericResponse<string>.NotFound("Leave record not found");
          }
          
          await _leaveRecordRepository.DeleteLeaveRecordAsync(leaveRecordId, ct);
          
          _logger.LogInformation("LeaveRecord with Id {Id} deleted successfully", leaveRecordId);
          
          return GenericResponse<string>.Success("Leave record deleted successfully", null, "200");
      }

      public async Task<GenericResponse<LeaveRecordDto>> GetLeaveRecordByIdAsync(int leaveRecordId, CancellationToken ct = default)
      {
          _logger.LogInformation("==============Inside {Method}==============", nameof(GetLeaveRecordByIdAsync));

          _logger.LogInformation("About to retrieve leave record with Id: {Id}", leaveRecordId);
          
          var existingLeaveRecord = await _leaveRecordRepository.GetLeaveRecordByIdAsync(leaveRecordId, ct);
          if (existingLeaveRecord is null)
          {
            _logger.LogWarning("LeaveRecord with Id {Id} not found", leaveRecordId);
            
            return GenericResponse<LeaveRecordDto>.NotFound("Leave record not found");
          }

          var leaveRecordToReturn = new LeaveRecordDto
          (
            existingLeaveRecord.Id,
            existingLeaveRecord.EmployeeId,
            existingLeaveRecord.Employee.FullName,
            existingLeaveRecord.StartDate,
            existingLeaveRecord.EndDate,
            existingLeaveRecord.Reason,
            existingLeaveRecord.Approved
          );
          
          _logger.LogInformation("LeaveRecord with Id {Id} retrieved successfully", leaveRecordId);
          
          return  GenericResponse<LeaveRecordDto>.Success("Leave record deleted successfully", leaveRecordToReturn, "200");
      }

      public async Task<GenericResponse<IEnumerable<LeaveRecordDto>>> GetLeaveRecordsByEmployeeIdAsync(int employeeId, CancellationToken ct = default)
      {
          _logger.LogInformation("==============Inside {Method}==============", nameof(GetLeaveRecordsByEmployeeIdAsync));

          _logger.LogInformation("About to retrieve leave records with employee Id: {Id}", employeeId);
          
          var existingLeaveRecords = await _leaveRecordRepository.GetLeaveRecordsByEmployeeIdAsync(employeeId, ct);
          if (!existingLeaveRecords.Any())
          {
              _logger.LogWarning("No existing leave record is found for employee with Id {Id}", employeeId);
            
              return GenericResponse<IEnumerable<LeaveRecordDto>>.NotFound("Leave records not found");
          }
          
        //  var leaveRecordsToReturn = new LeaveRecordDto()

        throw new NotImplementedException();
      }

      public async Task<GenericResponse<IEnumerable<LeaveRecordDto>>> GetLeaveRecordsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken ct = default)
      {
        throw new NotImplementedException();
      }

      public Task<GenericResponse<bool>> HasOverlappingLeaveAsync(int employeeId, DateTime startDate, DateTime endDate, CancellationToken ct = default)
      {
        throw new NotImplementedException();
      }

      public Task<GenericResponse<IEnumerable<LeaveRecordDto>>> GetPendingApprovalLeavesAsync(CancellationToken ct = default)
      {
        throw new NotImplementedException();
      }

      public Task<GenericResponse<string>> ApproveLeaveAsync(int leaveRecordId, bool approved, CancellationToken ct = default)
      {
        throw new NotImplementedException();
      }
}