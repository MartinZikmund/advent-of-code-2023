using var inputStream = File.OpenRead("input.txt");
using var reader = new StreamReader(inputStream);

var directions = reader.ReadLine()!;

reader.ReadLine();

var nodes = new Dictionary<string, Node>();

while (reader.ReadLine() is { } line)
{
    var parts = line.Split('=');
    var nodeName = parts[0].Trim();
    var destinations = parts[1].Trim().Trim('(', ')').Split(',');
    var left = destinations[0].Trim();
    var right = destinations[1].Trim();

    var node = new Node(nodeName, left, right);
    nodes[nodeName] = node;
}

long steps = 0;
var cycleSizes = new List<ulong>();

var startingNodes = new List<string>(nodes.Keys.Where(k => k.EndsWith('A')));
foreach (var startingNode in startingNodes)
{
    steps = 0;
    var currentNode = startingNode;
    var currentInstruction = 0;
    while (!currentNode.EndsWith('Z'))
    {
        var instruction = directions[currentInstruction];
        steps++;

        var node = nodes[currentNode];
        if (instruction == 'L')
        {
            currentNode = node.Left;
        }
        else
        {
            currentNode = node.Right;
        }

        currentInstruction++;
        currentInstruction %= directions.Length;
    }

    cycleSizes.Add((ulong)steps);
}

var lcm = cycleSizes[0];
for (int i = 1; i < cycleSizes.Count; i++)
{
    lcm = LowestCommonMultiple(lcm, cycleSizes[i]);
}

Console.WriteLine(lcm);

ulong LowestCommonMultiple(ulong a, ulong b)
{
    return a * b / GreatestCommonDivisor(a, b);
}

ulong GreatestCommonDivisor(ulong a, ulong b)
{
    while (b != 0)
    {
        var t = b;
        b = a % b;
        a = t;
    }

    return a;
}

record Node(string Name, string Left, string Right);