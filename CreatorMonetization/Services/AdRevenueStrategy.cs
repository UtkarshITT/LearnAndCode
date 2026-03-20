using CreatorMonetization.Core;
using CreatorMonetization.Models;

namespace CreatorMonetization.Services;

public sealed class AdRevenueStrategy : IEarningStrategy
{
	private readonly decimal amountPerView;
	private readonly IEarningAdjustment adjustment;

	public string Name => "Ad Revenue";

	public AdRevenueStrategy(decimal amountPerView, IEarningAdjustment adjustment)
	{
		if (amountPerView < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(amountPerView));
		}

		this.amountPerView = amountPerView;
		this.adjustment = adjustment ?? throw new ArgumentNullException(nameof(adjustment));
	}

	public decimal Calculate(MonetizationContext context)
	{
		var baseAmount = context.Views * amountPerView;
		return adjustment.Apply(baseAmount, context);
	}
}
