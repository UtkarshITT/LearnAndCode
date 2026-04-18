using System;
using System.Collections.Generic;
using Xunit;

namespace DivisorEqualityCounter.Core.Tests;

public class DivisorEqualityCounterServiceTests
{
	[Fact]
	public void countValidNumbersLessThanK_whenKIs15_returns2()
	{
		DivisorEqualityCounterService divisorEqualityCounterService = new();

		int result = divisorEqualityCounterService.countValidNumbersLessThanK(15);

		Assert.Equal(2, result);
	}

	[Fact]
	public void countValidNumbersLessThanK_whenKIs3_returns1()
	{
		DivisorEqualityCounterService divisorEqualityCounterService = new();

		int result = divisorEqualityCounterService.countValidNumbersLessThanK(3);

		Assert.Equal(1, result);
	}

	[Fact]
	public void countValidNumbersLessThanK_whenKIs2_returns0()
	{
		DivisorEqualityCounterService divisorEqualityCounterService = new();

		int result = divisorEqualityCounterService.countValidNumbersLessThanK(2);

		Assert.Equal(0, result);
	}

	[Fact]
	public void countValidNumbersForTestCases_whenInputHasMultipleValues_returnsExpectedCounts()
	{
		DivisorEqualityCounterService divisorEqualityCounterService = new();
		IReadOnlyList<int> inputValues = new List<int> { 15, 3, 2, 1 };

		IReadOnlyList<int> result = divisorEqualityCounterService.countValidNumbersForTestCases(inputValues);

		Assert.Equal(new List<int> { 2, 1, 0, 0 }, result);
	}

	[Fact]
	public void countValidNumbersLessThanK_whenKIsNegative_throwsException()
	{
		DivisorEqualityCounterService divisorEqualityCounterService = new();

		Assert.Throws<ArgumentException>(() =>
		{
			divisorEqualityCounterService.countValidNumbersLessThanK(-5);
		});
	}

	[Fact]
	public void countValidNumbersForTestCases_whenInputIsNull_throwsException()
	{
		DivisorEqualityCounterService divisorEqualityCounterService = new();

		Assert.Throws<ArgumentException>(() =>
		{
			divisorEqualityCounterService.countValidNumbersForTestCases(null!);
		});
	}
}
