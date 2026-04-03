using System.Collections.Generic;
using System.Threading.Tasks;
using OrderProcessingSystem.Models;

namespace OrderProcessingSystem.Interfaces
{
    public interface IPaymentGateway
    {
        Task<PaymentResult> ProcessPayment(string customerId, decimal amount, string paymentMethod);
        Task<bool> RefundPayment(string transactionId);
    }

    public interface IInventoryService
    {
        Task<bool> CheckAvailability(List<OrderItem> items);
        Task ReserveItems(List<OrderItem> items);
        Task CommitReservation(List<OrderItem> items);
        Task ReleaseReservation(List<OrderItem> items);
        Task RestoreInventory(List<OrderItem> items);
    }
    public interface INotificationService
    {
        Task SendOrderConfirmation(Order order);
    }
    public interface IOrderRepository
    {
        Task<Order> GetById(string orderId);
        Task Save(Order order);
    }
}
