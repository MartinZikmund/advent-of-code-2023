var input = File.ReadAllLines("input.txt");

Dictionary<string, Vertex> vertices = new();

foreach (var line in input)
{
    var parts = line.Split(':');
    var name = parts[0].Trim();
    var connections = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

    if (!vertices.ContainsKey(name))
    {
        vertices[name] = new Vertex(name);
    }

    foreach (var connection in connections)
    {
        if (!vertices.ContainsKey(connection))
        {
            vertices[connection] = new Vertex(connection);
        }

        vertices[name].Connections.Add(vertices[connection]);
        vertices[connection].Connections.Add(vertices[name]);
    }
}

var startVertex = vertices.Values.First();
for (int i = 0; i < 3; i++)
{
    var (farthestVertex, path) = FindFarthest(startVertex);

    for (int j = 1; j < path.Length; j++)
    {
        var from = path[j - 1];
        var to = path[j];
        from.Connections.Remove(to);
        to.Connections.Remove(from);
    }

    startVertex = farthestVertex;
}

var componentRepresentative = startVertex;
var componentSize = CalculateComponentSize(componentRepresentative);

Console.WriteLine(componentSize * (vertices.Count - componentSize));

(Vertex farthest, Vertex[] path) FindFarthest(Vertex source)
{
    var queue = new Queue<Vertex>();
    queue.Enqueue(source);
    var visited = new HashSet<Vertex>();
    var distances = new Dictionary<Vertex, int>();
    Vertex farthest = source;
    Dictionary<Vertex, Vertex?> previous = new();
    previous[source] = null;
    distances[source] = 0;

    while (queue.Count > 0)
    {
        var current = queue.Dequeue();

        if (visited.Contains(current))
        {
            continue;
        }

        visited.Add(current);

        foreach (var connection in current.Connections)
        {
            if (visited.Contains(connection))
            {
                continue;
            }

            distances[connection] = distances[current] + 1;
            previous[connection] = current;
            queue.Enqueue(connection);

            if (distances[connection] > distances[farthest])
            {
                farthest = connection;
            }
        }
    }

    var path = new List<Vertex>();
    var pathVertex = farthest;
    while (pathVertex != null)
    {
        path.Add(pathVertex);
        pathVertex = previous[pathVertex];
    }

    return (farthest, path.ToArray());
}

int CalculateComponentSize(Vertex componentRepresentative)
{
    var visited = new HashSet<Vertex>();
    var queue = new Queue<Vertex>();
    queue.Enqueue(componentRepresentative);
    visited.Add(componentRepresentative);
    while (queue.Count > 0)
    {
        var current = queue.Dequeue();
        foreach (var connection in current.Connections)
        {
            if (visited.Contains(connection))
            {
                continue;
            }

            visited.Add(connection);
            queue.Enqueue(connection);
        }
    }

    return visited.Count;
}

public record Vertex(string Name)
{
    public List<Vertex> Connections = new();
}