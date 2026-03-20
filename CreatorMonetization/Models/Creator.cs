using System.Collections.Generic;
using System.Linq;
using CreatorMonetization.Core;

namespace CreatorMonetization.Models;

public sealed class Creator
{
	private readonly List<IEarningStrategy> earningStrategies;

	public string Name { get; }

	public Creator(string name, IEnumerable<IEarningStrategy> earningStrategies)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Creator name is required.", nameof(name));
		}

		Name = name;
		this.earningStrategies = earningStrategies?.ToList() ?? new List<IEarningStrategy>();
	}

	public void AddEarningStrategy(IEarningStrategy strategy)
	{
		if (strategy is null)
		{
			throw new ArgumentNullException(nameof(strategy));
		}

		earningStrategies.Add(strategy);
	}

	public decimal CalculateTotalEarnings(MonetizationContext context)
	{
		if (context is null)
		{
			throw new ArgumentNullException(nameof(context));
		}

		return earningStrategies.Sum(strategy => strategy.Calculate(context));
	}
}
