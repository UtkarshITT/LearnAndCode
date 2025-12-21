using EmployeeSRP.Models;

namespace EmployeeSRP.Interfaces;
public interface IEmployeeRepository
{
    void Save(Employee employee);
    Employee? GetById(int id);
    List<Employee> GetAll();
    void Update(Employee employee);
}
