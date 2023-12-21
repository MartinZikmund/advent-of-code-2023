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

TiltUp(map, width, height);

for (var y = 0; y < height; y++)
{
    for (var x = 0; x < width; x++)
    {
        Console.Write(map[x, y]);
    }
    Console.WriteLine();
}

Console.WriteLine(CalculateLoad(map, width, height));

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