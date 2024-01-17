using Tools;

var input = File.ReadAllLines("input.txt");
var origin = new Point();
var currentPosition = origin;
var points = new List<Point>();
var totalBoundaryPoints = 0L;
foreach (var line in input)
{
    var parts = line.Split(' ');
    var direction = parts[0];
    var distance = int.Parse(parts[1]);
    Point newPosition = currentPosition;
    switch (direction)
    {
        case "R":
            newPosition = new Point(currentPosition.X + distance, currentPosition.Y);
            break;
        case "L":
            newPosition = new Point(currentPosition.X - distance, currentPosition.Y);
            break;
        case "U":
            newPosition = new Point(currentPosition.X, currentPosition.Y - distance);
            break;
        case "D":
            newPosition = new Point(currentPosition.X, currentPosition.Y + distance);
            break;
        default:
            throw new InvalidOperationException();
    }
    points.Add(newPosition);
    totalBoundaryPoints += Math.Abs(newPosition.X - currentPosition.X) + Math.Abs(newPosition.Y - currentPosition.Y);
    currentPosition = newPosition;
}

// Calculate area using Shoelace formula
long area = 0;
for (var currentPoint = 0; currentPoint < points.Count - 1; currentPoint++)
{
    var nextPoint = currentPoint + 1 == points.Count ? 0 : currentPoint + 1;
    area += points[currentPoint].X * points[nextPoint].Y - points[nextPoint].X * points[currentPoint].Y;
}
area = Math.Abs(area) / 2;

// Use Pick's theorem to calculate the number of points inside the polygon
var pointsInside = area - totalBoundaryPoints / 2 + 1;
Console.WriteLine($"Points inside: {pointsInside}");
Console.WriteLine($"Area: {area}");

Console.WriteLine("Total points: " + (totalBoundaryPoints + pointsInside));