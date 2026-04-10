namespace GeoLocationFetcher.Controllers;

using GeoLocationFetcher.Interfaces;
using GeoLocationFetcher.Validations;
using GeoLocationFetcher.Models;

public class LocationController
{
	private readonly ILocationService locationService;

	public LocationController(ILocationService service)
	{
		locationService = service;
	}

	public async Task<List<LocationResult>> getLocationAsync(string input)
	{
		if (!InputValidator.isValid(input))
		{
			throw new ArgumentException("Invalid input. Please enter a valid location.");
		}

		return await locationService.getLocationAsync(input);
	}
}
