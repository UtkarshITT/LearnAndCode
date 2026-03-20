namespace OOPsAssignments
{
	public sealed class Car : FuelVehicle
	{
		public Car(string make, string model, int year, double price, double fuelLevel)
			: base(make, model, year, price, fuelLevel)
		{
		}
	}
}
