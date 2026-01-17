using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentProcessing.Models;

namespace PaymentProcessing.Interfaces
{
    public interface ILogger
    {
        void Log(string message);
        void LogError(string message, Exception ex);
    }

    public interface INotificationService
    {
        Task SendAsync(string customerId, string message);
    }

    public interface IPaymentValidator
    {
        void Validate(PaymentRequest request);
    }

    public interface IPaymentRecorder
    {
        Task RecordTransactionAsync(string transactionId, PaymentRecord record);
        Task<List<PaymentRecord>> GetCustomerHistoryAsync(string customerId);
    }

    public interface IPaymentGateway
    {
        Task<bool> ProcessPaymentAsync(PaymentRequest request);
    }
}
