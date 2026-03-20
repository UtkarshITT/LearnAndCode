using System;
using System.Collections.Generic;
using System.Linq;

namespace OOPsAssignments
{
	public sealed class VehicleManager
	{
		private readonly List<Vehicle> vehicles = new List<Vehicle>();

		public IReadOnlyCollection<Vehicle> Vehicles => vehicles.AsReadOnly();

		public void AddVehicle(Vehicle vehicle)
		{
			if (vehicle == null)
			{
				throw new ArgumentNullException(nameof(vehicle));
			}

			vehicles.Add(vehicle);
			Console.WriteLine($"{vehicle.GetType().Name} added.");
		}

		public void DisplayAllVehicles()
		{
			if (vehicles.Count == 0)
			{
				Console.WriteLine("No vehicles available.");
				return;
			}

			foreach (var vehicle in vehicles)
			{
				vehicle.DisplayInfo();
			}
		}

		public double CalculateTotalValue()
		{
			return vehicles.Sum(vehicle => vehicle.Price);
		}

		public void StartAllVehicles()
		{
			foreach (var vehicle in vehicles)
			{
				vehicle.Start();
			}
		}
	}
}
