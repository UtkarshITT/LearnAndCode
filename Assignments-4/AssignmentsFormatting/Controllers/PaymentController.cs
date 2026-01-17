using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentProcessing.Interfaces;
using PaymentProcessing.Models;

namespace PaymentProcessing.Controllers
{
    public class PaymentController
    {
        private const int MAX_RETRIES = 3;
        private const string PAYMENT_SUCCESS = "Payment successful";
        private const string PAYMENT_FAILED = "Payment failed after all retry attempts";
        private readonly ILogger _logger;
        private readonly INotificationService _notificationService;
        private readonly IPaymentValidator _validator;
        private readonly IPaymentRecorder _recorder;
        private readonly IPaymentGateway _gateway;
        public PaymentController(
            ILogger logger,
            INotificationService notificationService,
            IPaymentValidator validator,
            IPaymentRecorder recorder,
            IPaymentGateway gateway)
        {
            _logger = logger;
            _notificationService = notificationService;
            _validator = validator;
            _recorder = recorder;
            _gateway = gateway;
        }
        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            _validator.Validate(request);

            int attempt = 0;

            while (attempt < MAX_RETRIES)
            {
                try
                {
                    await ExecutePaymentAsync(request);
                    await RecordTransactionAsync(request);
                    await NotifyCustomerOfSuccessAsync(request);

                    string transactionId = GenerateTransactionId();
                    return PaymentResult.Success(transactionId);
                }
                catch (PaymentException ex)
                {
                    attempt++;
                    
                    _logger.LogError(
                        $"Payment failed, retry attempt {attempt} of {MAX_RETRIES}",
                        ex
                    );

                    // Don't retry if we've exhausted all attempts
                    if (attempt >= MAX_RETRIES)
                    {
                        return PaymentResult.Failed(PAYMENT_FAILED);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(attempt));
                }
            }

            return PaymentResult.Failed(PAYMENT_FAILED);
        }
        public async Task<List<PaymentRecord>> GetCustomerHistoryAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new ArgumentException("Customer ID is required");
            }

            return await _recorder.GetCustomerHistoryAsync(customerId);
        }

        private async Task ExecutePaymentAsync(PaymentRequest request)
        {
            _logger.Log(
                $"Executing payment of {request.Amount:C} for customer {request.CustomerId}"
            );

            bool success = await _gateway.ProcessPaymentAsync(request);

            if (!success)
            {
                throw new PaymentException("Payment gateway returned failure");
            }
        }

        private async Task RecordTransactionAsync(PaymentRequest request)
        {
            string transactionId = GenerateTransactionId();

            PaymentRecord record = new PaymentRecord(
                request.CustomerId,
                request.Amount,
                DateTime.UtcNow
            );

            await _recorder.RecordTransactionAsync(transactionId, record);
        }

        private async Task NotifyCustomerOfSuccessAsync(PaymentRequest request)
        {
            string message = $"Payment of {request.Amount:C} processed successfully";
            await _notificationService.SendAsync(request.CustomerId, message);
        }

        private string GenerateTransactionId()
        {
            // Using Guid provides better uniqueness than timestamp
            return $"TXN-{Guid.NewGuid():N}";
        }
    }
}
