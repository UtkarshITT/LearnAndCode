using CreatorMonetization.Core;
using CreatorMonetization.Models;

namespace CreatorMonetization.Services;

public sealed class NoAdjustment : IEarningAdjustment
{
	public decimal Apply(decimal amount, MonetizationContext context)
	{
		return amount;
	}
}
