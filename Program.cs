using System;

namespace OOPsAssignments
{
	public static class Program
	{
		public static void Main()
		{
			RunDemo();
		}

		private static void RunDemo()
		{
			Console.WriteLine("=== Vehicle Management Demo ===\n");

			var vehicles = CreateSampleVehicles();
			DemonstrateIndividualVehicleBehavior(vehicles);

			var manager = RegisterVehicles(vehicles);
			DemonstrateVehicleManager(manager);
			DemonstrateEncapsulationValidation(vehicles.car);

			Console.WriteLine("\n=== Demo Complete ===");
		}

		private static (Car car, Motorcycle motorcycle, ElectricCar electricCar) CreateSampleVehicles()
		{
			var car = new Car(
				make: "Honda",
				model: "Accord",
				year: 2023,
				price: 28000,
				fuelLevel: 100);

			var motorcycle = new Motorcycle(
				make: "Harley-Davidson",
				model: "Street 750",
				year: 2022,
				price: 7500,
				fuelLevel: 80,
				hasSidecar: false);

			var electricCar = new ElectricCar(
				make: "Tesla",
				model: "Model 3",
				year: 2023,
				price: 42000,
				batteryLevel: 100);

			return (car, motorcycle, electricCar);
		}

		private static void DemonstrateIndividualVehicleBehavior(
			(Car car, Motorcycle motorcycle, ElectricCar electricCar) vehicles)
		{
			Console.WriteLine("Testing Vehicles:");
			vehicles.car.Start();
			vehicles.car.DisplayInfo();
			vehicles.car.Stop();

			Console.WriteLine();
			vehicles.motorcycle.Start();
			vehicles.motorcycle.DisplayInfo();

			Console.WriteLine();
			vehicles.electricCar.Start();
			vehicles.electricCar.DisplayInfo();
		}

		private static VehicleManager RegisterVehicles(
			(Car car, Motorcycle motorcycle, ElectricCar electricCar) vehicles)
		{
			var manager = new VehicleManager();
			manager.AddVehicle(vehicles.car);
			manager.AddVehicle(vehicles.motorcycle);
			manager.AddVehicle(vehicles.electricCar);
			return manager;
		}

		private static void DemonstrateVehicleManager(VehicleManager manager)
		{
			manager.DisplayAllVehicles();
			Console.WriteLine($"\nTotal Value: ${manager.CalculateTotalValue():N2}");

			Console.WriteLine("\nStarting all vehicles:");
			manager.StartAllVehicles();
		}

		private static void DemonstrateEncapsulationValidation(Car car)
		{
			Console.WriteLine("\n=== Encapsulation Validation ===");

			try
			{
				car.SetPrice(-1000);
			}
			catch (ArgumentOutOfRangeException exception)
			{
				Console.WriteLine($"Invalid price rejected: {exception.Message}");
			}

			Console.WriteLine($"Car price remains valid: ${car.Price:N2}");
			Console.WriteLine($"Car fuel remains valid: {car.FuelLevel:N1}%");
		}
	}
}
