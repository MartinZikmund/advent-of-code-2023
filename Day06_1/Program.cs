var input = File.ReadAllLines("input.txt");

var times = input[0]
        .Substring("Time:".Length)
        .Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(int.Parse)
        .ToArray();

var records = input[1]
        .Substring("Distance:".Length)
        .Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Select(int.Parse)
        .ToArray();

var total = 1;

for (int race = 0; race < times.Length; race++)
{
    var waysToWin = 0;
    var totalTime = times[race];
    var record = records[race];

    for (int time = 0; time < totalTime; time++)
    {
        if (time * (totalTime - time) > record)
        {
            waysToWin++;
        }
    }

    total *= waysToWin;
}

Console.WriteLine(total);