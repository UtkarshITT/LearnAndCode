using System;
using System.Threading.Tasks;
using OrderProcessingSystem.Interfaces;
using OrderProcessingSystem.Models;
using OrderProcessingSystem.Services;

namespace OrderProcessingSystem.Controllers
{
    public class OrderProcessor
    {

        private readonly IPaymentGateway _paymentGateway;
        private readonly IInventoryService _inventoryService;
        private readonly INotificationService _notificationService;
        private readonly IOrderRepository _orderRepository;
        private readonly OrderValidator _validator;


        public OrderProcessor(
            IPaymentGateway paymentGateway,
            IInventoryService inventoryService,
            INotificationService notificationService,
            IOrderRepository orderRepository)
        {
            _paymentGateway = paymentGateway;
            _inventoryService = inventoryService;
            _notificationService = notificationService;
            _orderRepository = orderRepository;
            _validator = new OrderValidator();
        }

        public async Task<OrderResult> ProcessOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (!_validator.IsValid(order))
                return OrderResult.Invalid("Order validation failed");

            bool hasInventory = await _inventoryService.CheckAvailability(order.Items);
            if (!hasInventory)
                return OrderResult.Failed("Insufficient inventory");

            await _inventoryService.ReserveItems(order.Items);

            try
            {
                var paymentResult = await _paymentGateway.ProcessPayment(
                    order.CustomerId,
                    order.TotalAmount,
                    order.PaymentMethod);

                if (paymentResult.IsSuccessful)
                {
                    await _inventoryService.CommitReservation(order.Items);
                    await _notificationService.SendOrderConfirmation(order);
                    return OrderResult.Success(paymentResult.TransactionId);
                }
                else
                {
                    await _inventoryService.ReleaseReservation(order.Items);
                    return OrderResult.Failed($"Payment failed: {paymentResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                // IMPORTANT: Release inventory reservation to prevent permanent locks
                // If we don't release here, inventory would remain reserved forever
                // even though the order failed, blocking other customers from purchasing
                await _inventoryService.ReleaseReservation(order.Items);
                Console.WriteLine($"Error processing order: {ex.Message}");
                throw;
            }
        }

        public async Task CancelOrder(string orderId)
        {
            var order = await _orderRepository.GetById(orderId);

            if (order == null)
                throw new ArgumentException($"Order {orderId} not found");

            // BUSINESS RULE: Only refund orders that were actually paid
            // Pending/unpaid orders can be cancelled without refund processing
            // This avoids unnecessary payment gateway calls and fees
            if (order.Status == OrderStatus.Paid)
            {
                await _paymentGateway.RefundPayment(order.TransactionId);
                await _inventoryService.RestoreInventory(order.Items);
            }

            order.Status = OrderStatus.Cancelled;
            await _orderRepository.Save(order);
        }
    }
}
