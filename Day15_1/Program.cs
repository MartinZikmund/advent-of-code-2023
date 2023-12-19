var input = File.ReadAllText("input.txt");

var parts = input.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

var runningTotal = 0;

foreach(var part in parts)
{
    runningTotal += Hash(part);
}

Console.WriteLine(runningTotal);

int Hash(string input)
{
    var value = 0;
    foreach (var c in input)
    {
        value += c;
        value *= 17;
        value %= 256;
    }

    return value;
}