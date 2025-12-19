namespace LSP.Shapes;
public class Square : IShape
{
    public double Side { get; set; }
    public string Name => "Square";

    public Square(double side)
    {
        Side = side;
    }

    public double CalculateArea()
    {
        return Side * Side;
    }
}
