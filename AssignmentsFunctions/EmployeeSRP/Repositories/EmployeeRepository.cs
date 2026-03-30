using EmployeeSRP.Interfaces;
using EmployeeSRP.Models;

namespace EmployeeSRP.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly List<Employee> _employees = new();

    public void Save(Employee employee)
    {
        _employees.Add(employee);
        Console.WriteLine($"[Database] Saved employee: {employee.Id} - {employee.Name}");
    }

    public Employee? GetById(int id)
    {
        Console.WriteLine($"[Database] Fetching employee with ID: {id}");
        return _employees.FirstOrDefault(e => e.Id == id);
    }

    public List<Employee> GetAll()
    {
        Console.WriteLine("[Database] Fetching all employees");
        return _employees.ToList();
    }

    public void Update(Employee employee)
    {
        var existing = _employees.FirstOrDefault(e => e.Id == employee.Id);
        if (existing != null)
        {
            existing.Name = employee.Name;
            existing.Department = employee.Department;
            existing.IsWorking = employee.IsWorking;
            Console.WriteLine($"[Database] Updated employee: {employee.Id} - {employee.Name}");
        }
    }
}
