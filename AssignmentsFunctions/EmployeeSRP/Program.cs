using EmployeeSRP.Interfaces;
using EmployeeSRP.Models;
using EmployeeSRP.Repositories;
using EmployeeSRP.Services;

/*
 * Employee SRP Example
 * 
 * Original Employee class violated SRP by having multiple responsibilities:
 *   - Employee data (id, name, department, working)
 *   - Database persistence (saveEmployeeTODatabase)
 *   - Report generation (printEmployeeDetailReportXML, printEmployeeDetailReportCSV)
 *   - Business operations (terminateEmployee, isWorking)
 * 
 * Refactored to follow SRP:
 *   - Employee: Only holds employee data
 *   - EmployeeRepository: Handles persistence
 *   - XmlReportService/CsvReportService: Handle report generation
 *   - EmployeeService: Handles business operations
 */

Console.WriteLine("EMPLOYEE SRP EXAMPLE");
Console.WriteLine("====================");
Console.WriteLine();
Console.WriteLine("Each class now has a SINGLE responsibility:");
Console.WriteLine("  - Employee: Hold employee data");
Console.WriteLine("  - EmployeeRepository: Save/update employees");
Console.WriteLine("  - XmlReportService: Generate XML reports");
Console.WriteLine("  - CsvReportService: Generate CSV reports");
Console.WriteLine("  - EmployeeService: Business operations (terminate, check status)");
Console.WriteLine();

// Initialize dependencies
IEmployeeRepository repository = new EmployeeRepository();
IEmployeeService employeeService = new EmployeeService(repository);
IEmployeeReportService xmlReportService = new XmlReportService();
IEmployeeReportService csvReportService = new CsvReportService();

// Create employees
var employee1 = new Employee(1, "John Doe", "Engineering");
var employee2 = new Employee(2, "Jane Smith", "Marketing");

// 1. Repository handles persistence (its responsibility)
Console.WriteLine("1. Persistence (EmployeeRepository's responsibility):");
Console.WriteLine("------------------------------------------------------");
repository.Save(employee1);
repository.Save(employee2);
Console.WriteLine();

// 2. Report services handle report generation (their responsibility)
Console.WriteLine("2. XML Report (XmlReportService's responsibility):");
Console.WriteLine("---------------------------------------------------");
Console.WriteLine(xmlReportService.GenerateReport(employee1));

Console.WriteLine("3. CSV Report (CsvReportService's responsibility):");
Console.WriteLine("---------------------------------------------------");
Console.WriteLine(csvReportService.GenerateReport(employee1));

// 3. Employee service handles business operations (its responsibility)
Console.WriteLine("4. Business Operations (EmployeeService's responsibility):");
Console.WriteLine("-----------------------------------------------------------");
Console.WriteLine($"Is {employee1.Name} working? {employeeService.CheckIfWorking(employee1)}");
employeeService.TerminateEmployee(employee1);
Console.WriteLine($"Is {employee1.Name} working? {employeeService.CheckIfWorking(employee1)}");
Console.WriteLine();

Console.WriteLine("Benefits of SRP:");
Console.WriteLine("  ✓ Employee class is simple - just data");
Console.WriteLine("  ✓ Can change database logic without touching Employee");
Console.WriteLine("  ✓ Can add new report formats (JSON, PDF) easily");
Console.WriteLine("  ✓ Each class is testable independently");
