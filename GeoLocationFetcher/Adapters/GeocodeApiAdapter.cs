namespace GeoLocationFetcher.Adapters;

using System.Text.Json;
using GeoLocationFetcher.Interfaces;
using GeoLocationFetcher.Models;
using GeoLocationFetcher.Config;

public class GeocodeApiAdapter : IGeocodingProvider
{
	private const string USER_AGENT = "GeoLocationFetcher/1.0";

	private readonly HttpClient httpClient;
	private readonly AppSettings settings;

	public GeocodeApiAdapter(AppSettings settings)
	{
		this.settings = settings;
		httpClient = new HttpClient();
		httpClient.DefaultRequestHeaders.Add("User-Agent", USER_AGENT);
	}

	public async Task<List<LocationResult>> getCoordinatesAsync(string location)
	{
		var requestUrl = buildUrl(location);
		var jsonResponse = await callApiAsync(requestUrl);
		return parseResponse(jsonResponse);
	}

	private string buildUrl(string location)
	{
		return $"{settings.baseUrl}?q={Uri.EscapeDataString(location)}&format=json";
	}

	private async Task<string> callApiAsync(string requestUrl)
	{
		var response = await httpClient.GetAsync(requestUrl);

		if (!response.IsSuccessStatusCode)
		{
			throw new HttpRequestException($"Geocoding API call failed with status code: {response.StatusCode}");
		}

		return await response.Content.ReadAsStringAsync();
	}

	private List<LocationResult> parseResponse(string jsonResponse)
	{
		var parsedLocations = new List<LocationResult>();

		using var document = JsonDocument.Parse(jsonResponse);
		var rootElement = document.RootElement;

		if (rootElement.ValueKind != JsonValueKind.Array)
		{
			return parsedLocations;
		}

		foreach (var element in rootElement.EnumerateArray())
		{
			double.TryParse(element.GetProperty("lat").GetString(), out double latitude);
			double.TryParse(element.GetProperty("lon").GetString(), out double longitude);

			parsedLocations.Add(new LocationResult
			{
				name = element.GetProperty("display_name").GetString() ?? string.Empty,
				latitude = latitude,
				longitude = longitude
			});
		}

		return parsedLocations;
	}
}
