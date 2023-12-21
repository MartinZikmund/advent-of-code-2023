var input = File.ReadAllLines("input.txt");

var width = input[0].Length;
var height = input.Length;

var map = new char[width, height];
var rotatedMap = new char[height, width];

for (var y = 0; y < height; y++)
{
    for (var x = 0; x < width; x++)
    {
        map[x, y] = input[y][x];
    }
}

Dictionary<int, long> visited = new();

var currentWidth = width;
var currentHeight = height;

for (var cycle = 0L; cycle < 1000000000; cycle++)
{
    var hash = HashMap(map, currentWidth, currentHeight);
    if (visited.ContainsKey(hash))
    {
        var cycleLength = cycle - visited[hash];
        var remainingCycles = 1000000000 - cycle;
        var cyclesToSkip = remainingCycles / cycleLength;
        cycle += cyclesToSkip * cycleLength;
        visited.Clear();
    }
    else
    {
        visited.Add(hash, cycle);
    }

    for (int i = 0; i < 4; i++)
    {
        TiltUp(map, width, height);
        RotateCw(map, rotatedMap, currentWidth, currentHeight);
        (map, rotatedMap) = (rotatedMap, map);
        (currentWidth, currentHeight) = (currentHeight, currentWidth);
    }
}

Console.WriteLine(CalculateLoad(map, width, height));
Console.ReadKey();

static int HashMap(char[,] map, int width, int height)
{
    var hash = 0;
    for (var y = 0; y < height; y++)
    {
        for (var x = 0; x < width; x++)
        {
            hash = (hash * 31) + map[x, y];
        }
    }

    return hash;
}

static void TiltUp(char[,] map, int width, int height)
{
    for (int x = 0; x < width; x++)
    {
        void OutputStones(int stones, int y)
        {
            for (int i = 0; i < stones; i++)
            {
                map[x, y + 1 + i] = 'O';
            }
        }

        // Start at the bottom of the column
        var stones = 0;
        var y = height - 1;
        while (y >= 0)
        {
            if (map[x, y] == 'O')
            {
                // Go up, count stones and replace with empty
                stones++;
                map[x, y] = '.';
            }
            else if (map[x, y] == '#')
            {
                OutputStones(stones, y);
                stones = 0;
            }
            y--;
        }

        OutputStones(stones, y);
    }
}

static int CalculateLoad(char[,] map, int width, int height)
{
    var load = 0;
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            if (map[x, y] == 'O')
            {
                load += (height - y);
            }
        }
    }

    return load;
}

static void RotateCw(char[,] source, char[,] target, int sourceWidth, int sourceHeight)
{
    var targetWidth = sourceHeight;
    var targetHeight = sourceWidth;
    for (var y = 0; y < sourceHeight; y++)
    {
        for (var x = 0; x < sourceWidth; x++)
        {
            target[targetWidth - y - 1, x] = source[x, y];
        }
    }
}