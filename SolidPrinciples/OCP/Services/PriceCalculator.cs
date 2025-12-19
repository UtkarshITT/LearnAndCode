using OCP.Discounts;

namespace OCP.Services;

public class PriceCalculator
{
    public decimal CalculateFinalPrice(decimal originalPrice, IDiscount discount)
    {
        var finalPrice = discount.Calculate(originalPrice);
        Console.WriteLine($"  Original: ${originalPrice:F2}");
        Console.WriteLine($"  Discount: {discount.Name}");
        Console.WriteLine($"  Final:    ${finalPrice:F2}");
        return finalPrice;
    }
}
