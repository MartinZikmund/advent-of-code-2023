using Tools;

var input = File.ReadAllLines("input.txt");

var width = input[0].Length;
var height = input.Length;

var map = new char[width, height];

for (var y = 0; y < height; y++)
{
    for (var x = 0; x < width; x++)
    {
        map[x, y] = input[y][x];
    }
}

var best = 0;
for (var x = 0; x < width; x++)
{
    best = Math.Max(best, CalculateEnergy(x, 0, (0, 1)));
    best = Math.Max(best, CalculateEnergy(x, height - 1, (0, -1)));
}

for (var y = 0; y < height; y++)
{
    best = Math.Max(best, CalculateEnergy(0, y, (1, 0)));
    best = Math.Max(best, CalculateEnergy(width - 1, y, (-1, 0)));
}

Console.WriteLine(best);

int CalculateEnergy(int x, int y, Point direction)
{
    var energized = new bool[width, height];
    var visited = new HashSet<(Point position, Point direction)>();

    var queue = new Queue<(Point position, Point direction)>();
    queue.Enqueue(((x, y), direction));
    while (queue.Count > 0)
    {
        var currentState = queue.Dequeue();

        if (currentState.position.X >= width ||
            currentState.position.X < 0 ||
            currentState.position.Y >= height ||
            currentState.position.Y < 0)
        {
            continue;
        }

        if (visited.Contains(currentState))
        {
            continue;
        }

        visited.Add(currentState);
        energized[currentState.position.X, currentState.position.Y] = true;

        var newDirection = currentState.direction;
        var character = map![currentState.position.X, currentState.position.Y];

        void Enqueue(Point direction)
        {
            queue.Enqueue((currentState.position + direction, direction));
        }

        if (character == '.')
        {
            Enqueue(currentState.direction);
        }
        else if (character == '/')
        {
            Enqueue(new Point(-currentState.direction.Y, -currentState.direction.X));
        }
        else if (character == '\\')
        {
            Enqueue(new Point(currentState.direction.Y, currentState.direction.X));
        }
        else if (character == '-')
        {
            if (currentState.direction.Y == 0)
            {
                Enqueue(currentState.direction);
            }
            else
            {
                Enqueue(new Point(-1, 0));
                Enqueue(new Point(1, 0));
            }
        }
        else if (character == '|')
        {
            if (currentState.direction.X == 0)
            {
                Enqueue(currentState.direction);
            }
            else
            {
                Enqueue(new Point(0, -1));
                Enqueue(new Point(0, 1));
            }
        }
    }

    //for (var yy = 0; yy < height; yy++)
    //{
    //    for (var xx = 0; xx < width; xx++)
    //    {
    //        Console.Write(energized[xx, yy] ? '#' : '.');
    //    }
    //    Console.WriteLine();
    //}

    return energized.Cast<bool>().Count(x => x);
}