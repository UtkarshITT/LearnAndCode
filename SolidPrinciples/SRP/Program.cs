using SRP.Repositories;
using SRP.Services;

Console.WriteLine("SINGLE RESPONSIBILITY PRINCIPLE (SRP) EXAMPLE");
Console.WriteLine("==============================================");
Console.WriteLine();
Console.WriteLine("Each class has a single responsibility:");
Console.WriteLine("  - User: Represents user data");
Console.WriteLine("  - UserRepository: Database operations");
Console.WriteLine("  - EmailService: Email sending");
Console.WriteLine("  - Logger: Logging");
Console.WriteLine("  - UserService: Orchestration");
Console.WriteLine();

var userRepository = new UserRepository();
var emailService = new EmailService();
var logger = new Logger();
var userService = new UserService(userRepository, emailService, logger);

Console.WriteLine("Creating a user:");
userService.CreateUser("John Doe", "john@example.com");
Console.WriteLine();

Console.WriteLine("Deleting a user:");
userService.DeleteUser("john@example.com");
