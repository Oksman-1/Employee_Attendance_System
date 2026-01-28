using System.Globalization;
using Attendance.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Attendance.Shared.Common;

public class AttendanceReportDocument : IDocument
{
    private readonly DateOnly _start;
    private readonly DateOnly _end;
    private readonly IReadOnlyList<AttendanceRecord> _attendanceRecords;
    
    public AttendanceReportDocument(DateOnly start, DateOnly end, IEnumerable<AttendanceRecord> attendanceRecords)
    {
        _start = start;
        _end = end;
        _attendanceRecords = attendanceRecords.ToList();
    }
    
    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    
    
    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(30);
            page.Size(PageSizes.A4);
            page.DefaultTextStyle(d => d.FontSize(10).FontColor(Colors.Black));

            page.Header().Column(header =>
            {
                header.Item().PaddingTop(2).Text($"Attendance Report").FontSize(18).Bold().FontColor(Colors.Blue.Medium);
                header.Item().Text($"From {_start:yyyy-MM-dd} to {_end:yyyy-MM-dd}").FontSize(14).Italic()
                    .FontColor(Colors.Grey.Darken1);
                header.Item().Text($"Generated on {DateTime.Now:yyyy-MM-dd HH:mm:ss}").FontSize(10)
                    .FontColor(Colors.Grey.Darken1);
            });
            
            page.Content().PaddingVertical(0).Element(ComposeContent);
            
            page.Footer().AlignCenter().Text(x =>
            {
                x.Span("Page ");
                x.CurrentPageNumber();
                x.Span(" of ");
                x.TotalPages().FontSize(9);
            });
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.Table(table =>
        {
            // Define columns
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(40); // ID
                columns.RelativeColumn(2);   // Employee Name
                columns.RelativeColumn(1);   // Date
                columns.RelativeColumn(1);   // Check-In Time
                columns.RelativeColumn(1);   // Check-Out Time
                columns.RelativeColumn(1);   // Hours Worked
            });
            
            // Header row
            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("ID").FontSize(12).SemiBold();
                header.Cell().Element(CellStyle).Text("Employee Name").FontSize(12).SemiBold();
                header.Cell().Element(CellStyle).Text("Date").FontSize(12).SemiBold();
                header.Cell().Element(CellStyle).Text("Check-In Time").FontSize(12).SemiBold();
                header.Cell().Element(CellStyle).Text("Check-Out Time").FontSize(12).SemiBold();
                header.Cell().Element(CellStyle).Text("Hours Worked").FontSize(12).SemiBold();

             
            });
            
            // Data rows
            var rowIndex = 1;
            foreach (var attendanceRecord in _attendanceRecords)
            {
                var attendanceDate = attendanceRecord.AttendanceDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                
                // Convert UTC times to local times
                var clockInTime = attendanceRecord?.ClockInAtUtc?.ToLocalTime().ToString("HH:mm:ss", CultureInfo.InvariantCulture) ?? "N/A";
                var clockOutTime = attendanceRecord?.ClockOutAtUtc?.ToLocalTime().ToString("HH:mm:ss", CultureInfo.InvariantCulture) ?? "N/A";
                
                var hoursWorked = attendanceRecord?.HoursWorked > 0 ? attendanceRecord.HoursWorked : (decimal)attendanceRecord!.CalculatedHoursWorked;
                
                table.Cell().Element(CellStyle).Text(rowIndex.ToString());
                table.Cell().Element(CellStyle).Text(attendanceRecord.Employee.FullName ?? string.Empty);
                table.Cell().Element(CellStyle).Text(attendanceDate);
                table.Cell().Element(CellStyle).Text(clockInTime);
                table.Cell().Element(CellStyle).Text(clockOutTime);
                table.Cell().Element(CellStyle).Text(hoursWorked.ToString("F2"));
                
                rowIndex++;
            }
        });
    }
     
    private static IContainer CellStyle(IContainer cell)
    {
        return cell.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1)
            .BorderColor(Colors.Black);
    }
}