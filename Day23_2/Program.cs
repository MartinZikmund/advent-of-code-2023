using Tools;

var input = File.ReadAllLines("input.txt");
var width = input[0].Length;
var height = input.Length;

var map = new char[width, height];

for (var y = 0; y < height; y++)
{
    for (var x = 0; x < width; x++)
    {
        map[x, y] = input[y][x] == '#' ? '#' : '.';
    }
}

var vertices = new Dictionary<Point, Vertex>();

var start = new Vertex(new(input[0].IndexOf('.'), 0));
var goal = new Vertex(new(input[^1].IndexOf('.'), height - 1));

vertices.Add(start.Position, start);
vertices.Add(goal.Position, goal);

var processingQueue = new Queue<Vertex>();
processingQueue.Enqueue(start);

var processedVertices = new HashSet<Vertex>();

while (processingQueue.Count > 0)
{
    var vertex = processingQueue.Dequeue();
    processedVertices.Add(vertex);
    FindNeighbors(vertex);
}

void FindNeighbors(Vertex source)
{
    HashSet<Point> visited = new();
    Dfs(source.Position, 0);

    void Dfs(Point position, int distance)
    {
        visited.Add(position);

        if (position == goal.Position)
        {
            source.Neighbors.Add(new Edge(source, goal, distance));
        }
        else
        {
            if (IsIntersection(position) && position != source.Position)
            {
                if (!vertices.ContainsKey(position))
                {
                    vertices.Add(position, new Vertex(position));
                    processingQueue.Enqueue(vertices[position]);
                }

                source.Neighbors.Add(new Edge(source, vertices[position], distance));
            }
            else
            {
                // >, <, ^, v, .
                foreach (var direction in Directions.WithoutDiagonals)
                {
                    var newPosition = position + direction;
                    if (!visited.Contains(newPosition) &&
                        IsValidMovement(position, newPosition))
                    {
                        Dfs(newPosition, distance + 1);
                    }
                }
            }
        }

        visited.Remove(position);
    }
}

var maximumDistance = 0;

var visited = new HashSet<Vertex>();
FindLongestPath(start, 0);

void FindLongestPath(Vertex current, int distance)
{
    visited.Add(current);

    if (current == goal)
    {
        maximumDistance = Math.Max(maximumDistance, distance);
    }
    else
    {
        foreach (var edge in current.Neighbors)
        {
            if (!visited.Contains(edge.To))
            {
                FindLongestPath(edge.To, distance + edge.Distance);
            }
        }
    }

    visited.Remove(current);
}

Console.WriteLine(maximumDistance);

bool IsValidMovement(Point from, Point to)
{
    if (!IsValid(to))
    {
        return false;
    }

    if (map[to.X, to.Y] == '#')
    {
        return false;
    }

    var sourceCharacter = map[from.X, from.Y];
    var targetCharacter = map[to.X, to.Y];

    return MatchesSlope(sourceCharacter, from, to) && MatchesSlope(targetCharacter, from, to);

    bool MatchesSlope(char slope, Point from, Point to)
    {
        if ((slope == '>' && (from.X + 1 != to.X || from.Y != to.Y)) ||
        (slope == '<' && (from.X - 1 != to.X || from.Y != to.Y)) ||
        (slope == '^' && (from.X != to.X || from.Y - 1 != to.Y)) ||
        (slope == 'v' && (from.X != to.X || from.Y + 1 != to.Y)))
        {
            return false;
        }

        return true;
    }
}

bool IsValid(Point position) =>
    position.X >= 0 && position.X < width && position.Y >= 0 && position.Y < height;

bool IsIntersection(Point position)
{
    if (!IsValid(position))
    {
        return false;
    }

    if (map[position.X, position.Y] == '#')
    {
        return false;
    }

    var nonWallNeighbors = 0;
    foreach (var direction in Directions.WithoutDiagonals)
    {
        if (IsValid(position + direction) && map[position.X + direction.X, position.Y + direction.Y] != '#')
        {
            nonWallNeighbors++;
        }
    }

    return nonWallNeighbors >= 3;
}

public class Vertex
{
    public Vertex(Point position)
    {
        Position = position;
    }

    public Point Position { get; }

    public List<Edge> Neighbors { get; } = new();
}

public record Edge(Vertex From, Vertex To, int Distance);