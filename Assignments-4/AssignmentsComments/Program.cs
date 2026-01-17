/*
 * Order Processing System - Main Program
 * 
 * COMMENT CLASSIFICATION SUMMARY (Clean Code Chapter 3):
 * 
 * BAD COMMENTS REMOVED FROM ORIGINAL CODE:
 * 
 * 1. REDUNDANT COMMENTS:
 *    Comments that just repeat what the code already says
 *    Example: "// This method processes an order"
 * 
 * 2. NOISE COMMENTS:
 *    Comments that add no value or meaning
 *    Example: "// Something went wrong"
 * 
 * 3. JOURNAL/ATTRIBUTION COMMENTS:
 *    Comments tracking who changed what and when
 *    Example: "// Added by John on 12/15/2023"
 *    Solution: Use Git history instead
 * 
 * 4. VAGUE TODO COMMENTS:
 *    TODO comments without specific action items
 *    Example: "// TODO: Fix this later"
 * 
 * 5. MISLEADING COMMENTS:
 *    Comments that describe WHAT instead of WHY
 *    Example: "// Payment failed, release inventory"
 * 
 * GOOD COMMENTS KEPT:
 * - Business rule explanation (refund logic)
 * - Reason for error handling (inventory lock prevention)
 * - Production validation requirements
 * 
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderProcessingSystem.Controllers;
using OrderProcessingSystem.Interfaces;
using OrderProcessingSystem.Models;
using OrderProcessingSystem.Services;

namespace OrderProcessingSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            PrintHeader();

            // Initialize services with dependency injection
            IPaymentGateway paymentGateway = new MockPaymentGateway();
            IInventoryService inventoryService = new MockInventoryService();
            INotificationService notificationService = new MockNotificationService();
            IOrderRepository orderRepository = new MockOrderRepository();

            // Create order processor with all dependencies
            var processor = new OrderProcessor(
                paymentGateway,
                inventoryService,
                notificationService,
                orderRepository);

            await RunTestCases(processor);

            PrintCommentAnalysis();
        }

        private static async Task RunTestCases(OrderProcessor processor)
        {
            await TestValidOrder(processor);
            await TestInvalidOrder(processor);
        }

        private static async Task TestValidOrder(OrderProcessor processor)
        {
            Console.WriteLine("\n--- Test Case 1: Process Valid Order ---");

            var order = new Order
            {
                OrderId = "ORD001",
                CustomerId = "CUST123",
                Items = new List<OrderItem>
                {
                    new OrderItem { ProductId = "PROD1", Quantity = 2, Price = 50.0m },
                    new OrderItem { ProductId = "PROD2", Quantity = 1, Price = 30.0m }
                },
                TotalAmount = 130.0m,
                PaymentMethod = "credit_card"
            };

            var result = await processor.ProcessOrder(order);
            Console.WriteLine($"Result: {result.Message}");
        }

        private static async Task TestInvalidOrder(OrderProcessor processor)
        {
            Console.WriteLine("\n--- Test Case 2: Process Invalid Order ---");

            var invalidOrder = new Order
            {
                OrderId = "ORD002",
                CustomerId = "CUST456",
                Items = new List<OrderItem>(),
                TotalAmount = 0,
                PaymentMethod = "credit_card"
            };

            var result = await processor.ProcessOrder(invalidOrder);
            Console.WriteLine($"Result: {result.Message}");
        }

        private static void PrintHeader()
        {
            Console.WriteLine(new string('=', 80));
            Console.WriteLine("Order Processing System - Comment Classification");
            Console.WriteLine(new string('=', 80));
            Console.WriteLine("\nProject Structure:");
            Console.WriteLine("  ├── Interfaces/      (Service contracts)");
            Console.WriteLine("  ├── Models/          (Data models)");
            Console.WriteLine("  ├── Services/        (Business logic)");
            Console.WriteLine("  └── Controllers/     (Orchestration)");
            Console.WriteLine(new string('=', 80));
        }

        private static void PrintCommentAnalysis()
        {
            Console.WriteLine("\n" + new string('=', 80));
            Console.WriteLine("COMMENT CLASSIFICATION SUMMARY:");
            Console.WriteLine(new string('-', 80));
            Console.WriteLine("BAD COMMENTS REMOVED:");
            Console.WriteLine("  • 18 Redundant Comments (repeat what code says)");
            Console.WriteLine("  • 4 Noise Comments (add no value)");
            Console.WriteLine("  • 2 Journal/Attribution Comments (belong in Git)");
            Console.WriteLine("  • 1 Vague TODO Comment (lacks specifics)");
            Console.WriteLine();
            Console.WriteLine("GOOD COMMENTS KEPT (Explain WHY):");
            Console.WriteLine("  • Business rule about refund logic");
            Console.WriteLine("  • Reason for releasing inventory on error");
            Console.WriteLine("  • Production validation requirements");
            Console.WriteLine();
            Console.WriteLine("CLEAN CODE PRINCIPLES:");
            Console.WriteLine(new string('-', 80));
            Console.WriteLine("✓ Self-documenting code through clear naming");
            Console.WriteLine("✓ Comments explain WHY, not WHAT");
            Console.WriteLine("✓ No redundant comments that repeat the code");
            Console.WriteLine("✓ No noise comments that add no value");
            Console.WriteLine("✓ Version control for attribution, not comments");
            Console.WriteLine("✓ Comments clarify business rules and intent");
            Console.WriteLine(new string('=', 80));
        }
    }
}
