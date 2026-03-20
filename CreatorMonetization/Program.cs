using System.Collections.Generic;
using CreatorMonetization.Core;
using CreatorMonetization.Models;
using CreatorMonetization.Services;
namespace CreatorMonetization;

public static class Program
{
	public static void Main()
	{
		var contextAwareAdjustment = new ContextAwareAdjustment(
			regionMultipliers: new Dictionary<Region, decimal>
			{
				[Region.NorthAmerica] = 1.20m,
				[Region.Europe] = 1.10m,
				[Region.AsiaPacific] = 1.05m,
				[Region.LatinAmerica] = 0.95m,
				[Region.Other] = 0.90m
			},
			seasonMultipliers: new Dictionary<Season, decimal>
			{
				[Season.Spring] = 1.00m,
				[Season.Summer] = 1.10m,
				[Season.Autumn] = 1.15m,
				[Season.Winter] = 1.25m
			},
			engagementBands: new List<EngagementBand>
			{
				new EngagementBand(0.03m, 0.90m),
				new EngagementBand(0.07m, 1.00m),
				new EngagementBand(0.12m, 1.15m)
			},
			defaultEngagementMultiplier: 1.30m);

		var creator = new Creator(
			"Utkarsh Sharma",
			new IEarningStrategy[]
			{
				new AdRevenueStrategy(amountPerView: 0.05m, adjustment: contextAwareAdjustment),
				new SubscriptionStrategy(amountPerSubscriber: 2.00m, adjustment: new NoAdjustment()),
				new BrandDealStrategy(baseDealAmount: 1200m, adjustment: contextAwareAdjustment)
			});

		creator.AddEarningStrategy(new LiveGiftStrategy(adjustment: new NoAdjustment()));

		var context = new MonetizationContext(
			views: 250000,
			subscribers: 4200,
			liveGiftCount: 350,
			liveGiftUnitValue: 1.5m,
			engagementRate: 0.10m,
			region: Region.NorthAmerica,
			season: Season.Winter);

		var totalEarnings = creator.CalculateTotalEarnings(context);
		Console.WriteLine($"Creator: {creator.Name}");
		Console.WriteLine($"Total earnings: {totalEarnings:C}");
	}
}
