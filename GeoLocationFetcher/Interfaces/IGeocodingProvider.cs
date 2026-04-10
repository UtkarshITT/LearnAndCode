namespace GeoLocationFetcher.Interfaces;

using GeoLocationFetcher.Models;

public interface IGeocodingProvider
{
	Task<List<LocationResult>> getCoordinatesAsync(string location);
}
