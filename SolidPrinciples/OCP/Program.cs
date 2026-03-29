using OCP.Discounts;
using OCP.Services;

Console.WriteLine("OPEN-CLOSED PRINCIPLE (OCP) EXAMPLE");
Console.WriteLine("====================================");
Console.WriteLine();
Console.WriteLine("OCP: Classes should be OPEN for extension, CLOSED for modification.");
Console.WriteLine("New discount types can be added without modifying PriceCalculator.");
Console.WriteLine();

var calculator = new PriceCalculator();
decimal productPrice = 200.00m;

Console.WriteLine("1. No Discount:");
calculator.CalculateFinalPrice(productPrice, new NoDiscount());
Console.WriteLine();

Console.WriteLine("2. Percentage Discount (15%):");
calculator.CalculateFinalPrice(productPrice, new PercentageDiscount(15));
Console.WriteLine();

Console.WriteLine("3. Fixed Discount ($50 off):");
calculator.CalculateFinalPrice(productPrice, new FixedDiscount(50));
Console.WriteLine();

Console.WriteLine("4. Seasonal Discount (added without modifying existing code!):");
calculator.CalculateFinalPrice(productPrice, new SeasonalDiscount());
Console.WriteLine();

Console.WriteLine("Benefits:");
Console.WriteLine("  - New discounts are added by creating new classes");
Console.WriteLine("  - PriceCalculator never needs to change");
Console.WriteLine("  - Existing code remains stable and tested");
