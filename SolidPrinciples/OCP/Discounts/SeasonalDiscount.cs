namespace OCP.Discounts;
public class SeasonalDiscount : IDiscount
{
    public string Name => "Seasonal Holiday Discount (25%)";

    public decimal Calculate(decimal price)
    {
        return price - (price * 0.25m);
    }
}
