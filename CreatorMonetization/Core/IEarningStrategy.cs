using CreatorMonetization.Models;

namespace CreatorMonetization.Core;

public interface IEarningStrategy
{
	string Name { get; }
	decimal Calculate(MonetizationContext context);
}
