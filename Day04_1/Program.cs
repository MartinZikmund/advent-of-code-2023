using Tools;

long runningTotal = 0;

var input = InputTools.ReadAllLines();
foreach(var line in input)
{
    var parts = line.Split(':');
    var numbers = parts[1].Split('|');
    var pickedNumbers = numbers[0]
        .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(int.Parse)
        .ToArray();
    var ourNumbers = numbers[1]
        .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(int.Parse)
        .ToArray();

    var matchCount = pickedNumbers.Intersect(ourNumbers).Count();
    if (matchCount == 0)
    {
        continue;
    }

    runningTotal += (1 << (matchCount - 1));
}

Console.WriteLine(runningTotal);