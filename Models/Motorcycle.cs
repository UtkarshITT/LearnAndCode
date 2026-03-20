using System;

namespace OOPsAssignments
{
	public sealed class Motorcycle : FuelVehicle
	{
		public Motorcycle(
			string make,
			string model,
			int year,
			double price,
			double fuelLevel,
			bool hasSidecar)
			: base(make, model, year, price, fuelLevel)
		{
			HasSidecar = hasSidecar;
		}

		public bool HasSidecar { get; }

		public override void DisplayInfo()
		{
			Console.WriteLine(
				$"Motorcycle: {Year} {Make} {Model}, Sidecar: {HasSidecar}, " +
				$"Price: ${Price:N2}, Fuel: {FuelLevel:N1}%, Running: {IsRunning}");
		}
	}
}
