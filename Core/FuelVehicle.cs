using System;

namespace OOPsAssignments
{
	public abstract class FuelVehicle : Vehicle
	{
		private double fuelLevel;

		protected FuelVehicle(string make, string model, int year, double price, double fuelLevel)
			: base(make, model, year, price)
		{
			SetFuelLevel(fuelLevel);
		}

		public double FuelLevel => fuelLevel;

		public override double EnergyLevel => FuelLevel;

		public override string EnergyUnit => "% fuel";

		public override void AddEnergy(double amount)
		{
			if (amount <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(amount), "Refuel amount must be positive.");
			}

			SetFuelLevel(FuelLevel + amount);
			Console.WriteLine($"Refueled. Fuel level: {FuelLevel:N1}%");
		}

		protected override bool CanStart()
		{
			return FuelLevel > 0;
		}

		private void SetFuelLevel(double value)
		{
			fuelLevel = ClampEnergyLevel(value);
		}
	}
}
