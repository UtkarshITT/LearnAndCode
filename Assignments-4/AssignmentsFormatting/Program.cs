using System;
using System.Threading.Tasks;
using PaymentProcessing.Controllers;
using PaymentProcessing.Interfaces;
using PaymentProcessing.Models;
using PaymentProcessing.Services;

namespace PaymentProcessing
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            PrintHeader();

            ILogger logger = new ConsoleLogger();
            INotificationService notificationService = new EmailNotificationService(logger);
            IPaymentValidator validator = new PaymentValidator();
            IPaymentRecorder recorder = new PaymentRecorder(logger);
            IPaymentGateway gateway = new PaymentGateway(logger);

            var paymentController = new PaymentController(
                logger,
                notificationService,
                validator,
                recorder,
                gateway
            );

            await RunTestCases(paymentController);

            PrintFooter();
        }

        private static async Task RunTestCases(PaymentController controller)
        {
            await TestValidPayment(controller);
            await TestPaymentExceedingLimit(controller);
            await TestInvalidAmount(controller);
            await TestCustomerHistory(controller);
        }

        private static async Task TestValidPayment(PaymentController controller)
        {
            Console.WriteLine("\n--- Test Case 1: Valid Payment ---");
            
            var request = new PaymentRequest(
                customerId: "CUST123",
                amount: 100.00m,
                paymentMethod: "credit_card"
            );

            var result = await controller.ProcessPaymentAsync(request);
            
            Console.WriteLine(
                $"Result: {result.Message} | Transaction ID: {result.TransactionId}"
            );
        }

        private static async Task TestPaymentExceedingLimit(PaymentController controller)
        {
            Console.WriteLine("\n--- Test Case 2: Payment Exceeding Limit ---");
            
            var request = new PaymentRequest(
                customerId: "CUST456",
                amount: 6000.00m,
                paymentMethod: "credit_card"
            );

            var result = await controller.ProcessPaymentAsync(request);
            Console.WriteLine($"Result: {result.Message}");
        }

        private static async Task TestInvalidAmount(PaymentController controller)
        {
            Console.WriteLine("\n--- Test Case 3: Invalid Amount ---");
            
            try
            {
                var request = new PaymentRequest(
                    customerId: "CUST789",
                    amount: 0.00m,
                    paymentMethod: "credit_card"
                );

                await controller.ProcessPaymentAsync(request);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Validation failed: {ex.Message}");
            }
        }

        private static async Task TestCustomerHistory(PaymentController controller)
        {
            Console.WriteLine("\n--- Test Case 4: Customer History ---");
            
            var history = await controller.GetCustomerHistoryAsync("CUST123");
            
            Console.WriteLine($"Found {history.Count} transaction(s) for customer CUST123");
            
            foreach (var record in history)
            {
                Console.WriteLine(
                    $"  - Amount: {record.Amount:C}, Date: {record.Timestamp:yyyy-MM-dd HH:mm:ss}"
                );
            }
        }

        private static void PrintHeader()
        {
            Console.WriteLine(new string('=', 80));
            Console.WriteLine("Payment Processing System - Clean Code Formatting");
            Console.WriteLine(new string('=', 80));
            Console.WriteLine("\nProject Structure:");
            Console.WriteLine("  ├── Interfaces/      (Service contracts)");
            Console.WriteLine("  ├── Models/          (Data models)");
            Console.WriteLine("  ├── Services/        (Business logic)");
            Console.WriteLine("  └── Controllers/     (Orchestration)");
            Console.WriteLine(new string('=', 80));
        }

        private static void PrintFooter()
        {
            Console.WriteLine("\n" + new string('=', 80));
            Console.WriteLine("FORMATTING PRINCIPLES DEMONSTRATED:");
            Console.WriteLine(new string('-', 80));
            Console.WriteLine("✓ Proper indentation and spacing (Horizontal Formatting)");
            Console.WriteLine("✓ Newspaper metaphor: high-level first, details later");
            Console.WriteLine("✓ Vertical openness: blank lines between concepts");
            Console.WriteLine("✓ Caller before callee ordering");
            Console.WriteLine("✓ Consistent line length (under 120 characters)");
            Console.WriteLine("✓ Related code grouped together");
            Console.WriteLine("\nSOLID PRINCIPLES APPLIED:");
            Console.WriteLine(new string('-', 80));
            Console.WriteLine("✓ Single Responsibility: Each class has one clear purpose");
            Console.WriteLine("✓ Open/Closed: Easy to extend without modification");
            Console.WriteLine("✓ Dependency Inversion: All dependencies use interfaces");
            Console.WriteLine(new string('=', 80));
        }
    }
}
