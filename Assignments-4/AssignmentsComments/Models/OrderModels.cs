using System.Collections.Generic;

namespace OrderProcessingSystem.Models
{
    public enum OrderStatus
    {
        Pending,
        Paid,
        Cancelled,
        Failed
    }
    public class OrderItem
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
    public class Order
    {
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public List<OrderItem> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string TransactionId { get; set; }
    }
    public class OrderResult
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public string TransactionId { get; set; }

        public static OrderResult Success(string transactionId)
        {
            return new OrderResult
            {
                IsSuccessful = true,
                Message = "Order processed successfully",
                TransactionId = transactionId
            };
        }

        public static OrderResult Failed(string reason)
        {
            return new OrderResult
            {
                IsSuccessful = false,
                Message = reason,
                TransactionId = null
            };
        }

        public static OrderResult Invalid(string reason)
        {
            return new OrderResult
            {
                IsSuccessful = false,
                Message = reason,
                TransactionId = null
            };
        }
    }
    public class PaymentResult
    {
        public bool IsSuccessful { get; set; }
        public string TransactionId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
