using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderProcessingSystem.Interfaces;
using OrderProcessingSystem.Models;

namespace OrderProcessingSystem.Services
{
    public class OrderValidator
    {
        public bool IsValid(Order order)
        {
            if (order.Items == null || order.Items.Count == 0)
                return false;

            if (order.TotalAmount <= 0)
                return false;

            // NOTE: Additional validation rules required in production:
            // - Customer credit limit validation (prevents over-spending)
            // - Payment method verification (ensures valid payment source)
            // - Shipping address validation (prevents delivery failures)
            return true;
        }
    }
    public class MockPaymentGateway : IPaymentGateway
    {
        public Task<PaymentResult> ProcessPayment(string customerId, decimal amount, string paymentMethod)
        {
            Console.WriteLine($"Processing payment: ${amount} for customer {customerId}");
            return Task.FromResult(new PaymentResult
            {
                IsSuccessful = true,
                TransactionId = "TXN123456"
            });
        }

        public Task<bool> RefundPayment(string transactionId)
        {
            Console.WriteLine($"Refunding transaction: {transactionId}");
            return Task.FromResult(true);
        }
    }

    public class MockInventoryService : IInventoryService
    {
        public Task<bool> CheckAvailability(List<OrderItem> items)
        {
            Console.WriteLine($"Checking availability for {items.Count} items");
            return Task.FromResult(true);
        }

        public Task ReserveItems(List<OrderItem> items)
        {
            Console.WriteLine("Items reserved");
            return Task.CompletedTask;
        }

        public Task CommitReservation(List<OrderItem> items)
        {
            Console.WriteLine("Reservation committed");
            return Task.CompletedTask;
        }

        public Task ReleaseReservation(List<OrderItem> items)
        {
            Console.WriteLine("Reservation released");
            return Task.CompletedTask;
        }

        public Task RestoreInventory(List<OrderItem> items)
        {
            Console.WriteLine("Inventory restored");
            return Task.CompletedTask;
        }
    }

    public class MockNotificationService : INotificationService
    {
        public Task SendOrderConfirmation(Order order)
        {
            Console.WriteLine($"Sending confirmation email for order {order.OrderId}");
            return Task.CompletedTask;
        }
    }

    public class MockOrderRepository : IOrderRepository
    {
        private readonly Dictionary<string, Order> _orders = new Dictionary<string, Order>();

        public Task<Order> GetById(string orderId)
        {
            _orders.TryGetValue(orderId, out var order);
            return Task.FromResult(order);
        }

        public Task Save(Order order)
        {
            _orders[order.OrderId] = order;
            Console.WriteLine($"Order {order.OrderId} saved with status {order.Status}");
            return Task.CompletedTask;
        }
    }
}
