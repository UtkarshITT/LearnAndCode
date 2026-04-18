using System;
using System.Collections.Generic;

namespace DivisorEqualityCounter.Core;

public class DivisorEqualityCounterService
{
	public int countValidNumbersLessThanK(int k)
	{
		if (k < 0)
		{
			throw new ArgumentException("k must be non-negative.", nameof(k));
		}

		if (k <= 2)
		{
			return 0;
		}

		int[] divisorCounts = calculateDivisorCountsUpTo(k);
		return countAdjacentPairsWithEqualDivisors(divisorCounts, k);
	}

	public IReadOnlyList<int> countValidNumbersForTestCases(IReadOnlyList<int> inputValues)
	{
		if (inputValues is null)
		{
			throw new ArgumentException("Input values are required.", nameof(inputValues));
		}

		List<int> results = new(inputValues.Count);
		for (int index = 0; index < inputValues.Count; index++)
		{
			int result = countValidNumbersLessThanK(inputValues[index]);
			results.Add(result);
		}

		return results;
	}

	private static int[] calculateDivisorCountsUpTo(int limit)
	{
		int[] divisorCounts = new int[limit + 1];
		for (int divisor = 1; divisor <= limit; divisor++)
		{
			for (int multiple = divisor; multiple <= limit; multiple += divisor)
			{
				divisorCounts[multiple]++;
			}
		}

		return divisorCounts;
	}

	private static int countAdjacentPairsWithEqualDivisors(int[] divisorCounts, int k)
	{
		int validPairCount = 0;
		for (int number = 2; number < k; number++)
		{
			if (divisorCounts[number] == divisorCounts[number + 1])
			{
				validPairCount++;
			}
		}

		return validPairCount;
	}
}
