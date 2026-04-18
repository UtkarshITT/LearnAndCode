using DivisorEqualityCounter.Core;

DivisorEqualityCounterService divisorEqualityCounterService = new();
IReadOnlyList<int> inputValues = readInputValues();
IReadOnlyList<int> results = divisorEqualityCounterService.countValidNumbersForTestCases(inputValues);
writeResults(results);

static IReadOnlyList<int> readInputValues()
{
	int testCaseCount = parseInteger(Console.ReadLine(), "test case count");
	List<int> inputValues = new(testCaseCount);
	for (int index = 0; index < testCaseCount; index++)
	{
		int currentValue = parseInteger(Console.ReadLine(), $"k at line {index + 1}");
		inputValues.Add(currentValue);
	}

	return inputValues;
}

static void writeResults(IReadOnlyList<int> results)
{
	for (int index = 0; index < results.Count; index++)
	{
		Console.WriteLine(results[index]);
	}
}

static int parseInteger(string? rawValue, string fieldName)
{
	if (rawValue is null || !int.TryParse(rawValue, out int parsedValue))
	{
		throw new ArgumentException($"Invalid {fieldName}.");
	}

	return parsedValue;
}
