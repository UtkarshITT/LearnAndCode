namespace GeoLocationFetcher.Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;
using GeoLocationFetcher.Models;

public interface ILocationService
{
	Task<List<LocationResult>> getLocationAsync(string location);
}
