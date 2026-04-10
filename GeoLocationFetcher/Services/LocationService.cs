namespace GeoLocationFetcher.Services;

using GeoLocationFetcher.Interfaces;
using GeoLocationFetcher.Models;

public class LocationService : ILocationService
{
	private readonly IGeocodingProvider geocodingProvider;

	public LocationService(IGeocodingProvider provider)
	{
		geocodingProvider = provider;
	}

	public async Task<List<LocationResult>> getLocationAsync(string location)
	{
		var fetchedLocations = await geocodingProvider.getCoordinatesAsync(location);
		ensureLocationsFound(fetchedLocations, location);
		return rankLocations(fetchedLocations, location);
	}

	private static void ensureLocationsFound(List<LocationResult> locations, string query)
	{
		if (locations.Count == 0)
		{
			throw new InvalidOperationException($"No results found for '{query}'.");
		}
	}

	private static List<LocationResult> rankLocations(IEnumerable<LocationResult> locations, string query)
	{
		return locations
			.Where(location => hasRelevantName(location, query))
			.GroupBy(location => location.name)
			.Select(locationGroup => locationGroup.First())
			.OrderByDescending(location => isExactMatch(location, query))
			.ThenByDescending(location => startsWithQuery(location, query))
			.ThenBy(location => location.name)
			.ToList();
	}

	private static bool hasRelevantName(LocationResult location, string query)
	{
		return !string.IsNullOrWhiteSpace(location.name)
			&& location.name.Contains(query, StringComparison.OrdinalIgnoreCase);
	}

	private static bool isExactMatch(LocationResult location, string query)
	{
		return location.name.Equals(query, StringComparison.OrdinalIgnoreCase);
	}

	private static bool startsWithQuery(LocationResult location, string query)
	{
		return location.name.StartsWith(query, StringComparison.OrdinalIgnoreCase);
	}
}
