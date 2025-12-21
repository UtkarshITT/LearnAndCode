using System.Text;
using EmployeeSRP.Interfaces;
using EmployeeSRP.Models;

namespace EmployeeSRP.Services;

public class CsvReportService : IEmployeeReportService
{
    private const string Header = "Id,Name,Department,IsWorking";

    public string GenerateReport(Employee employee)
    {
        var sb = new StringBuilder();
        sb.AppendLine(Header);
        sb.AppendLine($"{employee.Id},{employee.Name},{employee.Department},{employee.IsWorking}");
        return sb.ToString();
    }
}
