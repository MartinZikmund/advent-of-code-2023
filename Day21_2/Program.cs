using Tools;

const char Empty = '.';
const char Occupied = '#';
const char Start = 'S';

const long TotalSteps = 26501365;

var input = File.ReadAllLines("input.txt");
var mapSize = input[0].Length;
Point startPosition = default;
var map = new char[mapSize, mapSize];
for (var y = 0; y < mapSize; y++)
{
    for (var x = 0; x < mapSize; x++)
    {
        map[x, y] = input[y][x];
        if (input[y][x] == Start)
        {
            startPosition = new Point(x, y);
            map[x, y] = Empty;
        }
    }
}

checked
{
    var oddMapStepCount = CalculateReachable(startPosition, 10017);
    var evenMapStepCount = CalculateReachable(startPosition, 10020);

    long farthestMap = (TotalSteps - mapSize / 2) / (mapSize);
    var evenFullMapCount = farthestMap * farthestMap;
    var oddFullMapCount = (farthestMap - 1) * (farthestMap - 1);

    var oddStepsFull = oddFullMapCount * oddMapStepCount;
    var evenStepsFull = evenFullMapCount * evenMapStepCount;

    int smallTriangleSteps = (int)(TotalSteps - ((mapSize / 2) + (farthestMap - 1) * mapSize + (mapSize / 2) + 2));
    var smallTriangleTopRightReachable = CalculateReachable(new Point(0, mapSize - 1), smallTriangleSteps);
    var smallTriangleTopLeftReachable = CalculateReachable(new Point(mapSize - 1, mapSize - 1), smallTriangleSteps);
    var smallTriangleBottomRightReachable = CalculateReachable(new Point(0, 0), smallTriangleSteps);
    var smallTriangleBottomLeftReachable = CalculateReachable(new Point(mapSize - 1, 0), smallTriangleSteps);

    var smallTriangleStepsFull = (smallTriangleTopRightReachable + smallTriangleTopLeftReachable + smallTriangleBottomRightReachable + smallTriangleBottomLeftReachable) * farthestMap;

    int largeTriangleSteps = (int)(TotalSteps - ((mapSize / 2) + (farthestMap - 2) * mapSize + (mapSize / 2) + 2));
    var largeTriangleTopRightReachable = CalculateReachable(new Point(0, mapSize - 1), largeTriangleSteps);
    var largeTriangleTopLeftReachable = CalculateReachable(new Point(mapSize - 1, mapSize - 1), largeTriangleSteps);
    var largeTriangleBottomRightReachable = CalculateReachable(new Point(0, 0), largeTriangleSteps);
    var largeTriangleBottomLeftReachable = CalculateReachable(new Point(mapSize - 1, 0), largeTriangleSteps);

    var largeTriangleStepsFull = (largeTriangleTopRightReachable + largeTriangleTopLeftReachable + largeTriangleBottomRightReachable + largeTriangleBottomLeftReachable) * (farthestMap - 1);

    var edgeSteps = (int)(TotalSteps - ((mapSize / 2) + (farthestMap - 1) * mapSize + 1));
    var edgeTopReachable = CalculateReachable(new Point(mapSize / 2, mapSize - 1), edgeSteps);
    var edgeLeftReachable = CalculateReachable(new Point(mapSize - 1, mapSize / 2), edgeSteps);
    var edgeBottomReachable = CalculateReachable(new Point(mapSize / 2, 0), edgeSteps);
    var edgeRightReachable = CalculateReachable(new Point(0, mapSize / 2), edgeSteps);

    var edgeStepsFull = edgeTopReachable + edgeLeftReachable + edgeBottomReachable + edgeRightReachable;

    var result = oddStepsFull + evenStepsFull + smallTriangleStepsFull + largeTriangleStepsFull + edgeStepsFull;
    Console.WriteLine(result);
}

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
            if (newPosition.X < 0 || newPosition.X >= mapSize || newPosition.Y < 0 || newPosition.Y >= mapSize)
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