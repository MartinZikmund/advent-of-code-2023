var input = File.ReadAllLines("input.txt");
var origin = new Point(0, 0);
var currentPosition = origin;
var points = new List<Point>();
var totalBoundaryPoints = 0L;
foreach (var line in input)
{
    var parts = line.Split(' ');
    var encodedInstruction = parts[2].Trim('(').Trim(')').Trim('#');
    var direction = encodedInstruction.Last() - '0';
    var distance = Convert.ToInt32(encodedInstruction.Substring(0, encodedInstruction.Length - 1), 16);
    var newPosition = currentPosition;
    switch (direction)
    {
        case 0:
            newPosition = new Point(currentPosition.X + distance, currentPosition.Y);
            break;
        case 1:
            newPosition = new Point(currentPosition.X, currentPosition.Y + distance);
            break;
        case 2:
            newPosition = new Point(currentPosition.X - distance, currentPosition.Y);
            break;
        case 3:
            newPosition = new Point(currentPosition.X, currentPosition.Y - distance);
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

record Point(long X, long Y);