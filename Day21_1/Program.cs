using Tools;

const char Empty = '.';
const char Occupied = '#';
const char Start = 'S';

var input = File.ReadAllLines("input.txt");
var width = input[0].Length;
var height = input.Length;
Point startPosition = default;
var map = new char[width, height];
for (var y = 0; y < height; y++)
{
    for (var x = 0; x < width; x++)
    {   
        map[x, y] = input[y][x];
        if (input[y][x] == Start)
        {
            startPosition = new Point(x, y);
            map[x, y] = Empty;
        }
    }
}

var result = CalculateReachable(startPosition, 1000);
Console.WriteLine(result);

long CalculateReachable(Point startPosition, int steps)
{
    HashSet<Point> visited = new();
    HashSet<Point> reachable = new();
    Queue<(Point Position, int CurrentSteps)> queue = new();
    queue.Enqueue((startPosition, 0));
    visited.Add(startPosition);

    while (queue.Count > 0)
    {
        var (position, currentSteps) = queue.Dequeue();

        if (steps % 2 == currentSteps % 2)
        {
            reachable.Add(position);
        }

        if (currentSteps == steps)
        {
            continue;
        }

        foreach (var direction in Directions.WithoutDiagonals)
        {
            var newPosition = position + direction;
            if (newPosition.X < 0 || newPosition.X >= width || newPosition.Y < 0 || newPosition.Y >= height)
            {
                continue;
            }

            if (map[newPosition.X, newPosition.Y] == Empty && !visited.Contains(newPosition))
            {                
                visited.Add(newPosition);
                queue.Enqueue((newPosition, currentSteps + 1));
            }
        }
    }

    return reachable.Count;
}