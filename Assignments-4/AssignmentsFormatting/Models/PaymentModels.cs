using System;
using System.Collections.Generic;

namespace PaymentProcessing.Models
{
    public class PaymentRequest
    {
        public string CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }

        public PaymentRequest(string customerId, decimal amount, string paymentMethod)
        {
            CustomerId = customerId;
            Amount = amount;
            PaymentMethod = paymentMethod;
        }
    }
    public class PaymentResult
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public string TransactionId { get; set; }

        public PaymentResult(bool isSuccessful, string message, string transactionId)
        {
            IsSuccessful = isSuccessful;
            Message = message;
            TransactionId = transactionId;
        }

        public static PaymentResult Success(string transactionId)
        {
            return new PaymentResult(true, "Payment successful", transactionId);
        }

        public static PaymentResult Failed(string reason)
        {
            return new PaymentResult(false, reason, null);
        }
    }

    public class PaymentRecord
    {
        public string CustomerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }

        public PaymentRecord(string customerId, decimal amount, DateTime timestamp)
        {
            CustomerId = customerId;
            Amount = amount;
            Timestamp = timestamp;
        }
    }

    public class PaymentException : Exception
    {
        public PaymentException(string message) : base(message)
        {
        }

        public PaymentException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
