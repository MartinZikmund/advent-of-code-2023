using System.Text;

using var file = File.OpenText("input.txt");

long runningTotal = 0;

List<string> currentMap = new List<string>();
while (file.ReadLine() is { } line)
{
    if (line == string.Empty)
    {
        runningTotal += SolveWithSmudge(currentMap.ToArray());
        currentMap.Clear();
    }
    else
    {
        line = line.Replace("#", "1").Replace(".", "0");
        currentMap.Add(line);
    }
}

runningTotal += SolveWithSmudge(currentMap.ToArray());

Console.WriteLine(runningTotal);

int SolveWithSmudge(string[] map)
{
    int width = map[0].Length;
    int height = map.Length;
    char[,] charMap = new char[width, height];

    for (int y = 0; y < height; y++)
    {
        for (int col = 0; col < width; col++)
        {
            charMap[col, y] = map[y][col];
        }
    }

    var solutionWithoutSmudge = Solve(charMap, width, height, -1);

    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            charMap[x, y] = charMap[x, y] == '1' ? '0' : '1';

            var solution = Solve(charMap, width, height, solutionWithoutSmudge);
            if (solution != -1)
            {
                return solution;
            }

            charMap[x, y] = charMap[x, y] == '1' ? '0' : '1';
        }
    }

    throw new InvalidOperationException();
}

int Solve(char[,] map, int width, int height, int solutionWithoutSmudge)
{
    var invalidSolutionForRows = -1;
    var invalidSolutionForColumns = -1;
    if (solutionWithoutSmudge >= 100)
    {
        invalidSolutionForRows = solutionWithoutSmudge / 100 - 1;
    }
    else
    {
        invalidSolutionForColumns = solutionWithoutSmudge - 1;
    }

    // Horizontal
    var horizontalNumbers = new List<int>();
    for (int y = 0; y < height; y++)
    {
        var numberBuilder = new StringBuilder();
        for (int x = 0; x < width; x++)
        {
            numberBuilder.Append(map[x, y]);
        }
        var number = Convert.ToInt32(numberBuilder.ToString(), 2);
        horizontalNumbers.Add(number);
    }
    var horizontalIndex = FindReflectionIndex(horizontalNumbers.ToArray(), invalidSolutionForRows);
    if (horizontalIndex != -1)
    {
        var potentialSolution = (horizontalIndex + 1) * 100;
        if (potentialSolution != solutionWithoutSmudge)
        {
            return potentialSolution;
        }
    }

    // Vertical
    var verticalNumbers = new List<int>();
    for (int x = 0; x < width; x++)
    {
        var numberBuilder = new StringBuilder();
        for (int y = 0; y < height; y++)
        {
            numberBuilder.Append(map[x, y]);
        }
        var number = Convert.ToInt32(numberBuilder.ToString(), 2);
        verticalNumbers.Add(number);
    }

    var verticalIndex = FindReflectionIndex(verticalNumbers.ToArray(), invalidSolutionForColumns);
    if (verticalIndex != -1)
    {
        return (verticalIndex + 1);
    }

    return -1;
}

int FindReflectionIndex(int[] numbers, int invalidSolution)
{
    for (int i = 0; i < numbers.Length - 1; i++)
    {
        bool isReflection = true;
        int diff = 0;
        while (true)
        {
            var leftIndex = i - diff;
            var rightIndex = i + diff + 1;
            if (leftIndex < 0 || rightIndex > numbers.Length - 1)
            {
                break;
            }
            if (numbers[leftIndex] != numbers[rightIndex])
            {
                isReflection = false;
                break;
            }
            diff++;
        }

        if (isReflection && i != invalidSolution)
        {
            return i;
        }
    }

    return -1;
}