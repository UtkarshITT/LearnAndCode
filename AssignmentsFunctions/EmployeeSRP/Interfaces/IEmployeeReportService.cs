using EmployeeSRP.Models;

namespace EmployeeSRP.Interfaces;

public interface IEmployeeReportService
{
    string GenerateReport(Employee employee);
}
