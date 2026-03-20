namespace CreatorMonetization.Models;

public sealed class MonetizationContext
{
	public int Views { get; }
	public int Subscribers { get; }
	public int LiveGiftCount { get; }
	public decimal LiveGiftUnitValue { get; }
	public decimal EngagementRate { get; }
	public Region Region { get; }
	public Season Season { get; }

	public MonetizationContext(
		int views,
		int subscribers,
		int liveGiftCount,
		decimal liveGiftUnitValue,
		decimal engagementRate,
		Region region,
		Season season)
	{
		if (views < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(views));
		}

		if (subscribers < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(subscribers));
		}

		if (liveGiftCount < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(liveGiftCount));
		}

		if (liveGiftUnitValue < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(liveGiftUnitValue));
		}

		if (engagementRate < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(engagementRate));
		}

		Views = views;
		Subscribers = subscribers;
		LiveGiftCount = liveGiftCount;
		LiveGiftUnitValue = liveGiftUnitValue;
		EngagementRate = engagementRate;
		Region = region;
		Season = season;
	}
}
