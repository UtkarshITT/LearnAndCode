using CreatorMonetization.Core;
using CreatorMonetization.Models;

namespace CreatorMonetization.Services;

public sealed class BrandDealStrategy : IEarningStrategy
{
	private readonly decimal baseDealAmount;
	private readonly IEarningAdjustment adjustment;

	public string Name => "Brand Deal";

	public BrandDealStrategy(decimal baseDealAmount, IEarningAdjustment adjustment)
	{
		if (baseDealAmount < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(baseDealAmount));
		}

		this.baseDealAmount = baseDealAmount;
		this.adjustment = adjustment ?? throw new ArgumentNullException(nameof(adjustment));
	}

	public decimal Calculate(MonetizationContext context)
	{
		return adjustment.Apply(baseDealAmount, context);
	}
}
