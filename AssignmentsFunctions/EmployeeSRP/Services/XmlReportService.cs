using System.Text;
using EmployeeSRP.Interfaces;
using EmployeeSRP.Models;

namespace EmployeeSRP.Services;

public class XmlReportService : IEmployeeReportService
{
    public string GenerateReport(Employee employee)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<Employee>");
        sb.AppendLine($"  <Id>{employee.Id}</Id>");
        sb.AppendLine($"  <Name>{employee.Name}</Name>");
        sb.AppendLine($"  <Department>{employee.Department}</Department>");
        sb.AppendLine($"  <IsWorking>{employee.IsWorking}</IsWorking>");
        sb.AppendLine("</Employee>");
        return sb.ToString();
    }
}
