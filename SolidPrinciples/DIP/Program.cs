using DIP.Implementations;
using DIP.Services;

Console.WriteLine("DEPENDENCY INVERSION PRINCIPLE (DIP) EXAMPLE");
Console.WriteLine("=============================================");
Console.WriteLine();
Console.WriteLine("DIP: High-level modules should not depend on low-level modules.");
Console.WriteLine("     Both should depend on abstractions.");
Console.WriteLine();

// Scenario 1: Using Email + SQL Server + Console Logger
Console.WriteLine("1. NotificationService with Email, SQL Server, Console Logger:");
Console.WriteLine("----------------------------------------------------------------");
var emailService = new NotificationService(
    new EmailSender(),
    new SqlRepository(),
    new ConsoleLogger()
);
emailService.SendNotification("john@example.com", "Welcome to our platform!");
Console.WriteLine();

// Scenario 2: Using SMS + MongoDB + File Logger
Console.WriteLine("2. NotificationService with SMS, MongoDB, File Logger:");
Console.WriteLine("-------------------------------------------------------");
var smsService = new NotificationService(
    new SmsSender(),
    new MongoRepository(),
    new FileLogger()
);
smsService.SendNotification("+1234567890", "Your OTP is 123456");
Console.WriteLine();

// Scenario 3: Using Push Notification + SQL Server + Console Logger
Console.WriteLine("3. NotificationService with Push Notification:");
Console.WriteLine("-----------------------------------------------");
var pushService = new NotificationService(
    new PushNotificationSender(),
    new SqlRepository(),
    new ConsoleLogger()
);
pushService.SendBulkNotification(
    new[] { "user1", "user2", "user3" },
    "New feature available!"
);
Console.WriteLine();

Console.WriteLine("Benefits:");
Console.WriteLine("  - NotificationService doesn't know about concrete implementations");
Console.WriteLine("  - Easy to swap implementations (Email -> SMS -> Push)");
Console.WriteLine("  - Easy to test with mock implementations");
Console.WriteLine("  - Low-level modules can change without affecting high-level modules");
