using System.Drawing;

var input = File.ReadAllLines("input.txt");

var width = input[0].Length;
var height = input.Length;
var map = new char[width, height];
var gallaxies = new List<Point>();

for (int x = 0; x < input[0].Length; x++)
{
    for (int y = 0; y < input.Length; y++)
    {
        map[x, y] = input[y][x];
        if (map[x,y] == '#')
        {
            gallaxies.Add(new Point(x, y));
        }
    }
}

var emptyRows = new List<int>();
var emptyColumns = new List<int>();

for (int column = 0; column < width; column++)
{
    var isEmpty = true;
    for (int row = 0; row < height; row++)
    {
        if (map[column, row] != '.')
        {
            isEmpty = false;
            break;
        }
    }

    if (isEmpty)
    {
        emptyColumns.Add(column);
    }
}

for (int row = 0; row < height; row++)
{
    var isEmpty = true;
    for (int column = 0; column < width; column++)
    {
        if (map[column, row] != '.')
        {
            isEmpty = false;
            break;
        }
    }

    if (isEmpty)
    {
        emptyRows.Add(row);
    }
}

long totalDistance = 0;
foreach (var galaxy in gallaxies)
{
    foreach (var otherGalaxy in gallaxies)
    {
        if (galaxy == otherGalaxy)
        {
            continue;
        }

        var distance = Math.Abs(galaxy.X - otherGalaxy.X) + Math.Abs(galaxy.Y - otherGalaxy.Y);

        // Number of empty rows/columns between the two galaxies
        var emptyRowsBetween = emptyRows.Count(row => Math.Min(galaxy.Y, otherGalaxy.Y) < row && row < Math.Max(galaxy.Y, otherGalaxy.Y));
        var emptyColumnsBetween = emptyColumns.Count(column => Math.Min(galaxy.X, otherGalaxy.X) < column && column < Math.Max(galaxy.X, otherGalaxy.X));

        distance += emptyRowsBetween + emptyColumnsBetween;
        totalDistance += distance;
    }
}

Console.WriteLine(totalDistance / 2);