namespace OCP.Discounts;
public class PercentageDiscount : IDiscount
{
    private readonly decimal _percentage;

    public string Name => $"{_percentage}% Discount";

    public PercentageDiscount(decimal percentage)
    {
        _percentage = percentage;
    }

    public decimal Calculate(decimal price)
    {
        return price - (price * _percentage / 100);
    }
}
