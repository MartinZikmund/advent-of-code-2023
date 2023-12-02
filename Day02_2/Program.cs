var runningTotal = 0;

while (Console.ReadLine() is { } line)
{
    var gameInfo = line.Split(':');
    var gameId = int.Parse(gameInfo[0].Split(' ')[1]);
    var rounds = gameInfo[1].Split(';', StringSplitOptions.TrimEntries);
    var maxRed = 0;
    var maxGreen = 0;
    var maxBlue = 0;
    foreach (var round in rounds)
    {
        var colorInfos = round.Split(',', StringSplitOptions.TrimEntries);
        foreach (var color in colorInfos)
        {
            var colorInfo = color.Split(' ');
            var colorCount = int.Parse(colorInfo[0]);
            var colorName = colorInfo[1];
            switch (colorName)
            {
                case "red":
                    maxRed = Math.Max(colorCount, maxRed);
                    break;
                case "green":
                    maxGreen = Math.Max(colorCount, maxGreen);
                    break;
                case "blue":
                    maxBlue = Math.Max(colorCount, maxBlue);
                    break;
            }
        }
    }

    var product = maxRed * maxGreen * maxBlue;
    runningTotal += product;
}

Console.WriteLine(runningTotal);