using LSP.Shapes;
using LSP.Services;

Console.WriteLine("LISKOV SUBSTITUTION PRINCIPLE (LSP) EXAMPLE");
Console.WriteLine("============================================");
Console.WriteLine();
Console.WriteLine("LSP: Subtypes must be substitutable for their base types.");
Console.WriteLine("Any shape can be used wherever IShape is expected.");
Console.WriteLine();

var calculator = new AreaCalculator();

var shapes = new List<IShape>
{
    new Rectangle(10, 5),
    new Square(7),
    new Circle(4),
    new Triangle(8, 6)
};

calculator.PrintAllAreas(shapes);
Console.WriteLine();

double totalArea = calculator.CalculateTotalArea(shapes);
Console.WriteLine($"Total area of all shapes: {totalArea:F2}");
Console.WriteLine();

Console.WriteLine("Substitutability Demo:");
Console.WriteLine("----------------------");
IShape shape1 = new Rectangle(5, 3);
IShape shape2 = new Circle(5);
IShape shape3 = new Square(4);

calculator.PrintArea(shape1);
calculator.PrintArea(shape2);
calculator.PrintArea(shape3);
Console.WriteLine();

Console.WriteLine("Benefits:");
Console.WriteLine("  - Any IShape implementation works in AreaCalculator");
Console.WriteLine("  - New shapes can be added without changing existing code");
Console.WriteLine("  - No unexpected behavior when substituting types");
