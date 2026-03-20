using CreatorMonetization.Models;

namespace CreatorMonetization.Core;

public interface IEarningAdjustment
{
	decimal Apply(decimal amount, MonetizationContext context);
}
