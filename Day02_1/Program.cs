const int MaxRed = 12;
const int MaxGreen = 13;
const int MaxBlue = 14;

var runningTotal = 0;

while (Console.ReadLine() is { } line)
{
    var gameInfo = line.Split(':');
    var gameId = int.Parse(gameInfo[0].Split(' ')[1]);
    var rounds = gameInfo[1].Split(';', StringSplitOptions.TrimEntries);
    bool isGameValid = true;
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
                    if (colorCount > MaxRed)
                    {
                        isGameValid = false;
                    }
                    break;
                case "green":
                    if (colorCount > MaxGreen)
                    {
                        isGameValid = false;
                    }
                    break;
                case "blue":
                    if (colorCount > MaxBlue)
                    {
                        isGameValid = false;
                    }
                    break;
            }
        }

        if (!isGameValid)
        {
            break;
        }
    }

    if (isGameValid)
    {
        runningTotal += gameId;
    }
}

Console.WriteLine(runningTotal);