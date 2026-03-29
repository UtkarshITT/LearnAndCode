namespace OCP.Discounts;
public class FixedDiscount : IDiscount
{
    private readonly decimal _amount;

    public string Name => $"${_amount} Off";

    public FixedDiscount(decimal amount)
    {
        _amount = amount;
    }

    public decimal Calculate(decimal price)
    {
        var result = price - _amount;
        return result < 0 ? 0 : result;
    }
}
