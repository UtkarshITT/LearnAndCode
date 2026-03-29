using LSP.Shapes;

namespace LSP.Services;
public class AreaCalculator
{
    public void PrintArea(IShape shape)
    {
        Console.WriteLine($"  {shape.Name}: Area = {shape.CalculateArea():F2}");
    }

    public double CalculateTotalArea(IEnumerable<IShape> shapes)
    {
        double total = 0;
        foreach (var shape in shapes)
        {
            total += shape.CalculateArea();
        }
        return total;
    }

    public void PrintAllAreas(IEnumerable<IShape> shapes)
    {
        Console.WriteLine("Calculating areas for all shapes:");
        foreach (var shape in shapes)
        {
            PrintArea(shape);
        }
    }
}
