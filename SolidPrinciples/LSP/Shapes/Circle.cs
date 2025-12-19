namespace LSP.Shapes;
public class Circle : IShape
{
    public double Radius { get; set; }
    public string Name => "Circle";

    public Circle(double radius)
    {
        Radius = radius;
    }

    public double CalculateArea()
    {
        return Math.PI * Radius * Radius;
    }
}
