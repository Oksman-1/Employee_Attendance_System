using System.Threading.Tasks;

namespace Attendance.Application.Abstractions.Repositories;

public interface IEmployeeMigrationRepository
{
    Task UpdateEmptyPinHashesAsync(string defaultPinHash);
}
