using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Attendance");

            migrationBuilder.CreateTable(
                name: "Employees",
                schema: "Attendance",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMPLOYEE_CODE = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FULL_NAME = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DEPARTMENT = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    JOB_TITLE = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    QR_CODE = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    HIRE_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATED_AT = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ROW_VERSION = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                schema: "Attendance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    START_TIME = table.Column<TimeSpan>(type: "time", nullable: false),
                    END_TIME = table.Column<TimeSpan>(type: "time", nullable: false),
                    GRACE_PERIOD_MINUTES = table.Column<int>(type: "int", nullable: false, defaultValue: 10)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                    table.CheckConstraint("CK_Shift_StartEnd", "[END_TIME] > [START_TIME]");
                });

            migrationBuilder.CreateTable(
                name: "AttendanceRecords",
                schema: "Attendance",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ATTENDANCE_DATE = table.Column<DateOnly>(type: "date", nullable: false),
                    CLOCK_IN_UTC = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CLOCK_OUT_UTC = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NOTES = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    ROW_VERSION = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceRecords", x => x.Id);
                    table.CheckConstraint("CK_Attendance_ClockTimes_Valid", "[CLOCK_OUT_UTC] IS NULL OR [CLOCK_OUT_UTC] >= [CLOCK_IN_UTC]");
                    table.ForeignKey(
                        name: "FK_AttendanceRecords_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "Attendance",
                        principalTable: "Employees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaveRecords",
                schema: "Attendance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMPLOYEE_ID = table.Column<int>(type: "int", nullable: false),
                    START_DATE = table.Column<DateTime>(type: "DATE", nullable: false),
                    END_DATE = table.Column<DateTime>(type: "DATE", nullable: false),
                    REASON = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    APPROVED = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRecords", x => x.Id);
                    table.CheckConstraint("CK_LeaveRecord_DateRange", "[END_DATE] >= [START_DATE]");
                    table.ForeignKey(
                        name: "FK_LeaveRecords_Employees_EMPLOYEE_ID",
                        column: x => x.EMPLOYEE_ID,
                        principalSchema: "Attendance",
                        principalTable: "Employees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeShifts",
                schema: "Attendance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMPLOYEE_ID = table.Column<int>(type: "int", nullable: false),
                    SHIFT_ID = table.Column<int>(type: "int", nullable: false),
                    ASSIGNED_DATE = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeShifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeShifts_Employees_EMPLOYEE_ID",
                        column: x => x.EMPLOYEE_ID,
                        principalSchema: "Attendance",
                        principalTable: "Employees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeShifts_Shifts_SHIFT_ID",
                        column: x => x.SHIFT_ID,
                        principalSchema: "Attendance",
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecords_EmployeeId_ATTENDANCE_DATE",
                schema: "Attendance",
                table: "AttendanceRecords",
                columns: new[] { "EmployeeId", "ATTENDANCE_DATE" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EMAIL",
                schema: "Attendance",
                table: "Employees",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EMPLOYEE_CODE",
                schema: "Attendance",
                table: "Employees",
                column: "EMPLOYEE_CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_QR_CODE",
                schema: "Attendance",
                table: "Employees",
                column: "QR_CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShift_EmployeeId_AssignedDate",
                schema: "Attendance",
                table: "EmployeeShifts",
                columns: new[] { "EMPLOYEE_ID", "ASSIGNED_DATE" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeShifts_SHIFT_ID",
                schema: "Attendance",
                table: "EmployeeShifts",
                column: "SHIFT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRecords_EMPLOYEE_ID",
                schema: "Attendance",
                table: "LeaveRecords",
                column: "EMPLOYEE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_NAME",
                schema: "Attendance",
                table: "Shifts",
                column: "NAME",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceRecords",
                schema: "Attendance");

            migrationBuilder.DropTable(
                name: "EmployeeShifts",
                schema: "Attendance");

            migrationBuilder.DropTable(
                name: "LeaveRecords",
                schema: "Attendance");

            migrationBuilder.DropTable(
                name: "Shifts",
                schema: "Attendance");

            migrationBuilder.DropTable(
                name: "Employees",
                schema: "Attendance");
        }
    }
}
