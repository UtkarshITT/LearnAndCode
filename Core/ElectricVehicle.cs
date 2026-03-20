using System;

namespace OOPsAssignments
{
	public abstract class ElectricVehicle : Vehicle
	{
		private double batteryLevel;

		protected ElectricVehicle(string make, string model, int year, double price, double batteryLevel)
			: base(make, model, year, price)
		{
			SetBatteryLevel(batteryLevel);
		}

		public double BatteryLevel => batteryLevel;

		public override double EnergyLevel => BatteryLevel;

		public override string EnergyUnit => "% battery";

		public override void AddEnergy(double amount)
		{
			if (amount <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(amount), "Charge amount must be positive.");
			}

			SetBatteryLevel(BatteryLevel + amount);
			Console.WriteLine($"Charged. Battery level: {BatteryLevel:N1}%");
		}

		protected override bool CanStart()
		{
			return BatteryLevel > 0;
		}

		private void SetBatteryLevel(double value)
		{
			batteryLevel = ClampEnergyLevel(value);
		}
	}
}
