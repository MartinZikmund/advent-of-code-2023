using MathNet.Spatial.Euclidean;

//var testAreaMin = 7;
//var testAreaMax = 27;
var testAreaMin = 200000000000000;
var testAreaMax = 400000000000000;

var input = File.ReadAllLines("input.txt");

var lines = new List<Line2D>();

foreach (var line in input)
{
    var parts = line.Split('@', StringSplitOptions.TrimEntries);
    var positions = parts[0].Split(",", StringSplitOptions.TrimEntries);
    var velocities = parts[1].Split(",", StringSplitOptions.TrimEntries);
    var x = double.Parse(positions[0]);
    var y = double.Parse(positions[1]);

    var vx = double.Parse(velocities[0]);
    var vy = double.Parse(velocities[1]);

    lines.Add(new Line2D(new Point2D(x, y), new Point2D(x + vx, y + vy)));
}

var intersections = 0;

for (int firstLineId = 0; firstLineId < lines.Count; firstLineId++)
{
    for (int secondLineId = firstLineId + 1; secondLineId < lines.Count; secondLineId++)
    {
        var firstLine = lines[firstLineId];
        var secondLine = lines[secondLineId];
        if (firstLine.IntersectWith(secondLine) is { } intersection)
        {
            if (intersection.X >= testAreaMin &&
                intersection.Y >= testAreaMin &&
                intersection.X <= testAreaMax &&
                intersection.Y <= testAreaMax)
            {
                var firstIntersection = new Line2D(firstLine.StartPoint, intersection);
                var secondIntersection = new Line2D(secondLine.StartPoint, intersection);
                if (firstIntersection.Direction.Equals(firstLine.Direction, 0.0001) &&
                    secondIntersection.Direction.Equals(secondLine.Direction, 0.0001))
                {
                    intersections++;
                }
            }
        }
    }
}

Console.WriteLine(intersections);