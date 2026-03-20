using CreatorMonetization.Core;
using CreatorMonetization.Models;

namespace CreatorMonetization.Services;

public sealed class SubscriptionStrategy : IEarningStrategy
{
	private readonly decimal amountPerSubscriber;
	private readonly IEarningAdjustment adjustment;

	public string Name => "Subscription";

	public SubscriptionStrategy(decimal amountPerSubscriber, IEarningAdjustment adjustment)
	{
		if (amountPerSubscriber < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(amountPerSubscriber));
		}

		this.amountPerSubscriber = amountPerSubscriber;
		this.adjustment = adjustment ?? throw new ArgumentNullException(nameof(adjustment));
	}

	public decimal Calculate(MonetizationContext context)
	{
		var baseAmount = context.Subscribers * amountPerSubscriber;
		return adjustment.Apply(baseAmount, context);
	}
}
