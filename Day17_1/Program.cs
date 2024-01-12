using Tools;

var input = File.ReadAllLines("input.txt");
var width = input[0].Length;
var height = input.Length;
var map = new int[width, height];

for (int y = 0; y < height; y++)
{
    for (int x = 0; x < width; x++)
    {
        map[x, y] = input[y][x] - '0';
    }
}

var visited = new HashSet<Node>();
var minDistances = new Dictionary<Node, int>();
minDistances[new Node((0, 0), (0, 1), 0)] = 0;
minDistances[new Node((0, 0), (1, 0), 0)] = 0;
while (true)
{
    var minNode = minDistances.OrderBy(x => x.Value).FirstOrDefault();
    minDistances.Remove(minNode.Key);
    visited.Add(minNode.Key);
    var currentNode = minNode.Key;
    var totalHeat = minNode.Value;
    var direction = currentNode.Direction;
    var position = currentNode.Position;
    var directionSteps = currentNode.DirectionSteps;

    if (currentNode.Position == (width - 1, height - 1))
    {
        Console.WriteLine(totalHeat);
        break;
    }

    if (directionSteps < 3)
    {
        TryNextNode(direction, directionSteps + 1);
    }

    var leftTurn = (direction.Y, -direction.X);
    TryNextNode(leftTurn, 1);
    var rightTurn = (-direction.Y, direction.X);
    TryNextNode(rightTurn, 1);

    void TryNextNode(Point direction, int steps)
    {
        var nextPosition = new Point(position.X + direction.X, position.Y + direction.Y);
        if (nextPosition.X < 0 || nextPosition.X >= width || nextPosition.Y < 0 || nextPosition.Y >= height)
        {
            return;
        }

        int nextHeat = totalHeat + map[nextPosition.X, nextPosition.Y];
        var nextNode = new Node(nextPosition, direction, steps);

        if (visited.Contains(nextNode))
        {
            return;
        }

        if (!minDistances.ContainsKey(nextNode) || minDistances[nextNode] > nextHeat)
        {
            minDistances[nextNode] = nextHeat;
        }
    }
}

record Node(Point Position, Point Direction, int DirectionSteps);