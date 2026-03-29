namespace ObjectsDataStructures.PaymentSystem.Domain;

public sealed class Wallet
{
    private decimal _balance;

    public Wallet(decimal startingBalance)
    {
        if (startingBalance < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(startingBalance), "Starting balance cannot be negative.");
        }

        _balance = startingBalance;
    }

    public bool TryWithdraw(decimal amount)
    {
        if (amount <= 0)
        {
            return false;
        }

        if (_balance < amount)
        {
            return false;
        }

        _balance -= amount;
        return true;
    }

    public decimal GetBalance()
    {
        return _balance;
    }
}
