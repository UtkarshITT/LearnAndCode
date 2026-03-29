using ObjectsDataStructures.PaymentSystem.Domain;

namespace ObjectsDataStructures.PaymentSystem.Services;

public sealed class Paperboy
{
    public PaymentReceipt CollectPayment(Customer customer, decimal paymentAmount)
    {
        if (customer is null)
        {
            throw new ArgumentNullException(nameof(customer));
        }

        if (paymentAmount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(paymentAmount), "Payment amount must be positive.");
        }

        return customer.TryPay(paymentAmount);
    }
}
