using EmployeeSRP.Models;

namespace EmployeeSRP.Interfaces;

public interface IEmployeeService
{
    void TerminateEmployee(Employee employee);
    bool CheckIfWorking(Employee employee);
}
