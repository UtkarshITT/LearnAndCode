namespace EmployeeSRP.Models;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Department { get; set; }
    public bool IsWorking { get; set; }

    public Employee(int id, string name, string department, bool isWorking = true)
    {
        Id = id;
        Name = name;
        Department = department;
        IsWorking = isWorking;
    }
}
