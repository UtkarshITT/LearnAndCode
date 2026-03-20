using System.Collections.Generic;
using System.Linq;
using CreatorMonetization.Core;
using CreatorMonetization.Models;

namespace CreatorMonetization.Services;

public sealed class ContextAwareAdjustment : IEarningAdjustment
{
	private readonly IReadOnlyDictionary<Region, decimal> regionMultipliers;
	private readonly IReadOnlyDictionary<Season, decimal> seasonMultipliers;
	private readonly IReadOnlyList<EngagementBand> engagementBands;
	private readonly decimal defaultEngagementMultiplier;

	public ContextAwareAdjustment(
		IReadOnlyDictionary<Region, decimal> regionMultipliers,
		IReadOnlyDictionary<Season, decimal> seasonMultipliers,
		IReadOnlyList<EngagementBand> engagementBands,
		decimal defaultEngagementMultiplier)
	{
		this.regionMultipliers = regionMultipliers;
		this.seasonMultipliers = seasonMultipliers;
		this.engagementBands = engagementBands.OrderBy(band => band.MaxRateInclusive).ToList();
		this.defaultEngagementMultiplier = defaultEngagementMultiplier;
	}

	public decimal Apply(decimal amount, MonetizationContext context)
	{
		var regionMultiplier = regionMultipliers.GetValueOrDefault(context.Region, 1m);
		var seasonMultiplier = seasonMultipliers.GetValueOrDefault(context.Season, 1m);
		var engagementMultiplier = ResolveEngagementMultiplier(context.EngagementRate);
		return amount * regionMultiplier * seasonMultiplier * engagementMultiplier;
	}

	private decimal ResolveEngagementMultiplier(decimal engagementRate)
	{
		var matchedBand = engagementBands.FirstOrDefault(band => engagementRate <= band.MaxRateInclusive);
		return matchedBand?.Multiplier ?? defaultEngagementMultiplier;
	}
}
