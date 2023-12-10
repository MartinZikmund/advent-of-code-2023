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

int steps = 0;
var currentInstruction = 0;
var currentNode = "AAA";
while (currentNode != "ZZZ")
{
    var instruction = directions[currentInstruction];
    currentInstruction++;
    currentInstruction %= directions.Length;
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
}

Console.WriteLine(steps);

record Node(string Name, string Left, string Right);