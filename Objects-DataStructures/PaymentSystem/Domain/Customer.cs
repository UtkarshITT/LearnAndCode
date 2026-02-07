namespace ObjectsDataStructures.PaymentSystem.Domain;

public sealed class Customer
{
    private readonly Wallet _wallet;

    public Customer(string firstName, string lastName, Wallet wallet)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name is required.", nameof(firstName));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name is required.", nameof(lastName));
        }

        _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; }

    public string LastName { get; }

    public PaymentReceipt TryPay(decimal amount)
    {
        var balanceBefore = _wallet.GetBalance();
        var wasPaid = _wallet.TryWithdraw(amount);
        var balanceAfter = _wallet.GetBalance();

        return new PaymentReceipt(wasPaid, amount, balanceBefore, balanceAfter);
    }
}
