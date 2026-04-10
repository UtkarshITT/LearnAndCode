namespace GeoLocationFetcher.Application;

using GeoLocationFetcher.Controllers;

public class App
{
	private readonly LocationController locationController;

	public App(LocationController controller)
	{
		locationController = controller;
	}

	public async Task runAsync()
	{
		while (true)
		{
			var input = readInput();

			if (shouldExit(input))
			{
				break;
			}

			if (string.IsNullOrWhiteSpace(input))
			{
				continue;
			}

			await fetchAndPrintResultsAsync(input);
		}
	}

	private string readInput()
	{
		Console.WriteLine("\n=== GeoLocation Fetcher ===");
		Console.Write("Enter location (or type 'exit' to quit): ");
		return Console.ReadLine() ?? string.Empty;
	}

	private static bool shouldExit(string input)
	{
		return input.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase);
	}

	private async Task fetchAndPrintResultsAsync(string input)
	{
		try
		{
			var results = await locationController.getLocationAsync(input);
			printResults(results);
		}
		catch (Exception exception)
		{
			Console.WriteLine($"Error: {exception.Message}");
		}
	}

	private static void printResults(IEnumerable<Models.LocationResult> results)
	{
		var index = 1;

		foreach (var result in results)
		{
			Console.WriteLine($"\nResult {index++}:");
			Console.WriteLine($"Name: {result.name}");
			Console.WriteLine($"Latitude: {result.latitude}");
			Console.WriteLine($"Longitude: {result.longitude}");
		}
	}
}
