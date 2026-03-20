namespace CreatorMonetization.Models;

public sealed class EngagementBand
{
	public decimal MaxRateInclusive { get; }
	public decimal Multiplier { get; }

	public EngagementBand(decimal maxRateInclusive, decimal multiplier)
	{
		if (maxRateInclusive < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(maxRateInclusive));
		}

		if (multiplier <= 0)
		{
			throw new ArgumentOutOfRangeException(nameof(multiplier));
		}

		MaxRateInclusive = maxRateInclusive;
		Multiplier = multiplier;
	}
}
