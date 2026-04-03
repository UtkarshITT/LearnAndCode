using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentProcessing.Interfaces;
using PaymentProcessing.Models;

namespace PaymentProcessing.Services
{
    public class PaymentValidator : IPaymentValidator
    {
        private const decimal MIN_AMOUNT = 0.01m;

        public void Validate(PaymentRequest request)
        {
            ValidateCustomerId(request);
            ValidateAmount(request);
            ValidatePaymentMethod(request);
        }

        private void ValidateCustomerId(PaymentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.CustomerId))
            {
                throw new ArgumentException("Customer ID is required");
            }
        }

        private void ValidateAmount(PaymentRequest request)
        {
            if (request.Amount < MIN_AMOUNT)
            {
                throw new ArgumentException(
                    $"Payment amount must be at least {MIN_AMOUNT:C}"
                );
            }
        }

        private void ValidatePaymentMethod(PaymentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.PaymentMethod))
            {
                throw new ArgumentException("Payment method is required");
            }

            // TODO: Additional validation for payment method could be added here
            // For example: validate credit card format, expiry date, etc.
        }
    }
    public class PaymentRecorder : IPaymentRecorder
    {
        private readonly Dictionary<string, PaymentRecord> _database;
        private readonly ILogger _logger;

        public PaymentRecorder(ILogger logger)
        {
            _logger = logger;
            _database = new Dictionary<string, PaymentRecord>();
        }

        public Task RecordTransactionAsync(string transactionId, PaymentRecord record)
        {
            _database[transactionId] = record;
            
            _logger.Log(
                $"Transaction {transactionId} recorded for customer {record.CustomerId}"
            );
            
            return Task.CompletedTask;
        }

        public Task<List<PaymentRecord>> GetCustomerHistoryAsync(string customerId)
        {
            var history = _database.Values
                .Where(record => record.CustomerId == customerId)
                .OrderByDescending(record => record.Timestamp)
                .ToList();

            return Task.FromResult(history);
        }
    }

    public class PaymentGateway : IPaymentGateway
    {
        private const decimal PAYMENT_LIMIT = 5000.00m;
        private readonly ILogger _logger;

        public PaymentGateway(ILogger logger)
        {
            _logger = logger;
        }

        public Task<bool> ProcessPaymentAsync(PaymentRequest request)
        {
            _logger.Log(
                $"Processing payment of {request.Amount:C} for customer {request.CustomerId}"
            );

            if (request.Amount > PAYMENT_LIMIT)
            {
                throw new PaymentException(
                    $"Payment amount exceeds limit of {PAYMENT_LIMIT:C}, approval required"
                );
            }

            return Task.FromResult(true);
        }
    }
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [INFO] {message}");
        }

        public void LogError(string message, Exception ex)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [ERROR] {message}");
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }

    public class EmailNotificationService : INotificationService
    {
        private readonly ILogger _logger;

        public EmailNotificationService(ILogger logger)
        {
            _logger = logger;
        }

        public Task SendAsync(string customerId, string message)
        {
            _logger.Log($"Sending email to customer {customerId}: {message}");
            
            return Task.CompletedTask;
        }
    }
}
