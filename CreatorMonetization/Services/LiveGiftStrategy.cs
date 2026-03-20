using CreatorMonetization.Core;
using CreatorMonetization.Models;

namespace CreatorMonetization.Services;

public sealed class LiveGiftStrategy : IEarningStrategy
{
	private readonly IEarningAdjustment adjustment;

	public string Name => "Live Gift";

	public LiveGiftStrategy(IEarningAdjustment adjustment)
	{
		this.adjustment = adjustment ?? throw new ArgumentNullException(nameof(adjustment));
	}

	public decimal Calculate(MonetizationContext context)
	{
		var baseAmount = context.LiveGiftCount * context.LiveGiftUnitValue;
		return adjustment.Apply(baseAmount, context);
	}
}
