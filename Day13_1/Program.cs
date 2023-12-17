using System.Text;

using var file = File.OpenText("input.txt");

long runningTotal = 0;

List<string> currentMap = new List<string>();
while (file.ReadLine() is { } line)
{
    if (line == string.Empty)
    {
        runningTotal += Solve(currentMap.ToArray());
        currentMap.Clear();
    }
    else
    {
        line = line.Replace("#", "1").Replace(".", "0");
        currentMap.Add(line);
    }
}

runningTotal += Solve(currentMap.ToArray());

Console.WriteLine(runningTotal);

int Solve(string[] map)
{
    // Horizontal
    var horizontalNumbers = new List<int>();
    for (int i = 0; i < map.Length; i++)
    {
        var number = Convert.ToInt32(map[i], 2);
        horizontalNumbers.Add(number);
    }
    var horizontalIndex = FindReflectionIndex(horizontalNumbers.ToArray());
    if (horizontalIndex != -1)
    {
        return (horizontalIndex + 1) * 100;
    }

    // Vertical
    var verticalNumbers = new List<int>();
    for (int column = 0; column < map[0].Length; column++)
    {
        var numberBuilder = new StringBuilder();
        for (int row = 0; row < map.Length; row++)
        {
            numberBuilder.Append(map[row][column]);
        }
        var number = Convert.ToInt32(numberBuilder.ToString(), 2);
        verticalNumbers.Add(number);
    }

    var verticalIndex = FindReflectionIndex(verticalNumbers.ToArray());
    if (verticalIndex != -1)
    {
        return (verticalIndex + 1);
    }

    throw new InvalidOperationException("No solution found");
}

int FindReflectionIndex(int[] numbers)
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

        if (isReflection)
        {
            return i;
        }
    }

    return -1;
}