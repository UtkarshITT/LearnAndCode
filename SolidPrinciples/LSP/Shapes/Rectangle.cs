namespace LSP.Shapes;
public class Rectangle : IShape
{
    public virtual double Width { get; set; }
    public virtual double Height { get; set; }
    public string Name => "Rectangle";

    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public double CalculateArea()
    {
        return Width * Height;
    }
}
