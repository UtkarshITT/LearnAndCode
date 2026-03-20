using System;

namespace OOPsAssignments
{
	public abstract class Vehicle
	{
		private const int FirstValidProductionYear = 1886;
		private const int FutureYearAllowance = 1;
		private const double MinPrice = 0;
		private const double MaxPrice = 1_000_000;
		protected const double MinEnergyLevel = 0;
		protected const double MaxEnergyLevel = 100;

		private readonly string make;
		private readonly string model;
		private readonly int year;
		private double price;
		private bool isRunning;

		protected Vehicle(string make, string model, int year, double price)
		{
			if (string.IsNullOrWhiteSpace(make))
			{
				throw new ArgumentException("Make is required.", nameof(make));
			}

			if (string.IsNullOrWhiteSpace(model))
			{
				throw new ArgumentException("Model is required.", nameof(model));
			}

			if (year < FirstValidProductionYear || year > DateTime.UtcNow.Year + FutureYearAllowance)
			{
				throw new ArgumentOutOfRangeException(nameof(year), "Year is out of valid range.");
			}

			this.make = make.Trim();
			this.model = model.Trim();
			this.year = year;
			SetPrice(price);
		}

		public string Make => make;

		public string Model => model;

		public int Year => year;

		public double Price => price;

		public bool IsRunning => isRunning;

		public void SetPrice(double value)
		{
			if (value < MinPrice || value > MaxPrice)
			{
				throw new ArgumentOutOfRangeException(nameof(value), "Price must be between 0 and 1,000,000.");
			}

			price = value;
		}

		public bool Start()
		{
			if (!CanStart())
			{
				Console.WriteLine($"Cannot start {GetVehicleLabel()} - no energy available.");
				return false;
			}

			isRunning = true;
			Console.WriteLine($"{GetVehicleLabel()} started.");
			return true;
		}

		public void Stop()
		{
			if (!isRunning)
			{
				Console.WriteLine($"{GetVehicleLabel()} is already stopped.");
				return;
			}

			isRunning = false;
			Console.WriteLine($"{GetVehicleLabel()} stopped.");
		}

		public abstract void AddEnergy(double amount);

		public abstract double EnergyLevel { get; }

		public abstract string EnergyUnit { get; }

		public virtual void DisplayInfo()
		{
			Console.WriteLine(
				$"{GetType().Name}: {Year} {Make} {Model}, Price: ${Price:N2}, " +
				$"Energy: {EnergyLevel:N1}{EnergyUnit}, Running: {IsRunning}");
		}

		protected abstract bool CanStart();

		protected static double ClampEnergyLevel(double value)
		{
			return Math.Clamp(value, MinEnergyLevel, MaxEnergyLevel);
		}

		protected string GetVehicleLabel()
		{
			return $"{Make} {Model}";
		}
	}
}
